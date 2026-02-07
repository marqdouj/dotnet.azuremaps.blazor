import atlas from "azure-maps-control";
import { Helpers } from "../../common/Helpers";
import { EventsLogger } from "./EventsLogger";
import { EventsHelper } from "./EventsHelper";
import { IMapReference, IMapEventDef } from "../../typings";
import { EventNotification } from "../AzEvents";
import { AzPopups } from "../AzPopups";

export class PopupEvents {
    add(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        this.#addPopupEvents(mapRef, this.#getEventDefs(events));
    }

    remove(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        this.#removePopupEvents(mapRef, this.#getEventDefs(events));
    }

    #getEventDefs(events: IMapEventDef[]) {
        return Object.values(events).filter(value => value.target === "popup" && Helpers.isValueInEnum(MapPopupEvent, value.type));
    }

    #addPopupEvents(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addPopupEvents";

        events.forEach((value) => {
            const target = this.#getTarget(mapRef, value);
            let wasAdded: boolean = false;

            if (target) {
                const callback = this.#getCallback(mapRef, value, false);

                if (callback) {
                    if (value.once) {
                        mapRef.map.events.addOnce(value.type as MapPopupEvent, target, callback);
                    }
                    else {
                        mapRef.map.events.add(value.type as MapPopupEvent, target, callback);
                    }
                    wasAdded = true;
                }

                EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
            }
            else {
                EventsLogger.logInvalidTargetId(mapRef.mapId, eventName, value);
            }
        });
    }

    #removePopupEvents(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removePopupEvents";

        events.forEach((value) => {
            const target = this.#getTarget(mapRef, value);
            let wasRemoved: boolean = false;

            if (target) {
                const callback = this.#getCallback(mapRef, value, true);

                if (callback) {
                    mapRef.map.events.remove(value.type as MapPopupEvent, target, callback);
                    wasRemoved = true;
                }

                EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
            }
            else {
                EventsLogger.logInvalidTargetId(mapRef.mapId, eventName, value);
            }
        });
    }

    #getCallback(mapRef: IMapReference, value: IMapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        callback = (callback: atlas.TargetedEvent) => this.#NotifyMapPopupEvent(callback, mapRef, value);

        mapRef.eventsMap.addCallback(value, callback);
        return callback;
    }

    #getTarget(mapRef: IMapReference, event: IMapEventDef) {
        return AzPopups.getPopup(mapRef, event.targetId);
    }

    #NotifyMapPopupEvent = (callback: atlas.TargetedEvent, mapRef: IMapReference, event: IMapEventDef) => {
        let result = EventsHelper.buildMapEventArgs(mapRef.mapId, event, { popup: { type: callback.type } });
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEvent, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEvent, event.type);
    };
}

enum MapPopupEvent {
    Drag = 'drag',
    DragEnd = 'dragend',
    DragStart = 'dragstart',
    Open = 'open',
    Close = 'close',
}