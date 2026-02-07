import atlas from "azure-maps-control";
import { Helpers } from "../../common/Helpers";
import { EventsLogger } from "./EventsLogger";
import { EventsHelper } from "./EventsHelper";
import { IMapReference, IMapEventDef } from "../../typings";
import { EventNotification } from "../AzEvents";
import { AzMarkers } from "../AzMarkers";

export class MarkerEvents {
    add(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        this.#addHtmlMarkerEvents(mapRef, this.#getEventDefs(events));
    }

    remove(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        this.#removeHtmlMarkerEvents(mapRef, this.#getEventDefs(events));
    }

    #getEventDefs(events: IMapEventDef[]) {
        return Object.values(events).filter(value => value.target === "htmlmarker" && Helpers.isValueInEnum(MapHtmlMarkerEvent, value.type));
    }

    #addHtmlMarkerEvents(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addHtmlMarkerEvents";

        events.forEach((value) => {
            const target = this.#getTarget(mapRef, value);
            let wasAdded: boolean = false;

            if (target) {
                const eventType = value.type as MapHtmlMarkerEvent;
                const callback = this.#getCallback(mapRef, value, false);

                if (callback) {
                    switch (eventType) {
                        case MapHtmlMarkerEvent.KeyDown:
                        case MapHtmlMarkerEvent.KeyPress:
                        case MapHtmlMarkerEvent.KeyUp:
                            //NOTE: I've added code to create the events as an example of what should normally work,
                            //      however, the map does not pass the keyboard events to html markers.
                            const element = target.getElement();
                            if (element) {
                                element.tabIndex = 0;
                                element.addEventListener(eventType, callback);
                                wasAdded = true;
                            }
                            break;
                        default:
                            if (value.once) {
                                mapRef.map.events.addOnce(eventType, target, callback);
                            }
                            else {
                                mapRef.map.events.add(eventType, target, callback);
                            }
                            wasAdded = true;

                            break;
                    }
                }

                EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
            }
            else {
                EventsLogger.logInvalidTargetId(mapRef.mapId, eventName, value);
            }
        });
    }

    #removeHtmlMarkerEvents(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeHtmlMarkerEvents";

        events.forEach((value) => {
            const target = this.#getTarget(mapRef, value);
            let wasRemoved: boolean = false;

            if (target) {

                const callback = this.#getCallback(mapRef, value, true);

                if (callback) {
                    const eventType = value.type as MapHtmlMarkerEvent;

                    switch (eventType) {
                        case MapHtmlMarkerEvent.KeyDown:
                        case MapHtmlMarkerEvent.KeyPress:
                        case MapHtmlMarkerEvent.KeyUp:
                            const element = target.getElement();
                            if (element) {
                                element.removeEventListener(eventType, callback);
                            }
                            break;
                        default:
                            mapRef.map.events.remove(value.type as MapHtmlMarkerEvent, target, callback);
                    }
                    
                    wasRemoved = true;
                }

                EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
            }
            else {
                EventsLogger.logInvalidTargetId(mapRef.mapId, eventName, value);
            }
        });
    }

    #getCallback(mapRef: IMapReference, event: IMapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(event, removing);

        if (callback) {
            return callback;
        }

        const eventType = event.type as MapHtmlMarkerEvent;

        switch (eventType) {
            case MapHtmlMarkerEvent.KeyDown:
            case MapHtmlMarkerEvent.KeyPress:
            case MapHtmlMarkerEvent.KeyUp:
                callback = (callback: KeyboardEvent) => this.#notifyMapHtmlMarkerKeyboardEvent(callback, mapRef, event);
                break;
            default:
                callback = (callback: atlas.TargetedEvent) => this.#notifyMapHtmlMarkerEvent(callback, mapRef, event);
        }

        mapRef.eventsMap.addCallback(event, callback);
        return callback;
    }

    #getTarget(mapRef: IMapReference, event: IMapEventDef) {
        return AzMarkers.getMarker(mapRef.mapId, event.targetId);
    }

    #notifyMapHtmlMarkerEvent = (callback: atlas.TargetedEvent, mapRef: IMapReference, event: IMapEventDef) => {
        let result = EventsHelper.buildMapEventArgs(mapRef.mapId, event, { htmlMarker: { type: callback.type } });
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEvent, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEvent, event.type);
    };

    #notifyMapHtmlMarkerKeyboardEvent = (callback: KeyboardEvent, mapRef: IMapReference, event: IMapEventDef) => {
        if (event.preventDefault) {
            callback.preventDefault();
        }
        let result = EventsHelper.buildMapEventArgs(mapRef.mapId, event, { htmlMarker: { type: callback.type, ...EventsHelper.buildKeyboardEventPayload(callback) } });
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEvent, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEvent, event.type);
    };
}

enum MapHtmlMarkerEvent {
    Click = 'click',
    ContextMenu = 'contextmenu',
    DblClick = 'dblclick',
    MouseDown = 'mousedown',
    MouseEnter = 'mouseenter',
    MouseLeave = 'mouseleave',
    MouseMove = 'mousemove',
    MouseOut = 'mouseout',
    MouseOver = 'mouseover',
    MouseUp = 'mouseup',

    Drag = 'drag',
    DragEnd = 'dragend',
    DragStart = 'dragstart',

    KeyDown = 'keydown',
    KeyPress = 'keypress',
    KeyUp = 'keyup',
}