import * as atlas from "azure-maps-control"
import { Helpers } from "../../common/Helpers";
import { EventsLogger } from "./EventsLogger";
import { EventsHelper } from "./EventsHelper";
import { IMapReference, IMapEventDef } from "../../typings";
import { EventNotification } from "../AzEvents";

export class LayerEvents {
    add(mapRef: IMapReference, events: IMapEventDef[], layer?: atlas.layer.Layer) {
        if (events.length == 0) return;

        this.#addLayerEvents(mapRef, this.#getEventDefs(events), layer);
    }

    remove(mapRef: IMapReference, events: IMapEventDef[], layer?: atlas.layer.Layer) {
        if (events.length == 0) return;

        this.#removeLayerEvents(mapRef, this.#getEventDefs(events), layer);
    }

    #getEventDefs(events: IMapEventDef[]) {
        return Object.values(events).filter(value => value.target === "layer" && Helpers.isValueInEnum(MapLayerEvent, value.type));
    }

    #addLayerEvents(mapRef: IMapReference, events: IMapEventDef[], layer?: atlas.layer.Layer) {
        if (events.length == 0) return;

        const eventName = "addLayerEvents";
        const azmap = mapRef.map;

        events.forEach((value) => {
            let wasAdded: boolean = false;
            const target = layer ?? this.#getTarget(azmap, value);

            if (layer) {
                value.targetId = layer.getId();
            }

            if (!target) {
                EventsLogger.logInvalidTargetId(mapRef.mapId, eventName, value);
                return;
            }

            const callback = this.#getCallback(mapRef, value, false);

            if (callback) {
                if (target) {
                    if (value.once) {
                        azmap.events.addOnce(value.type as any, target, callback as any);
                    }
                    else {
                        azmap.events.add(value.type as any, target, callback as any);
                    }
                    wasAdded = true;
                }
            }

            EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
        });
    }

    #removeLayerEvents(mapRef: IMapReference, events: IMapEventDef[], layer?: atlas.layer.Layer) {
        if (events.length == 0) return;

        const eventName = "removeLayerEvents";
        const azmap = mapRef.map;

        events.forEach((value) => {
            let wasRemoved: boolean = false;
            const target = layer ?? this.#getTarget(azmap, value);

            if (layer) {
                value.targetId = layer.getId();
            }

            if (!target) {
                EventsLogger.logInvalidTargetId(mapRef.mapId, eventName, value);
                return;
            }

            const callback = this.#getCallback(mapRef, value, true);

            if (callback) {
                if (target) {
                    azmap.events.remove(value.type, target, callback);
                    wasRemoved = true;
                }
                else {
                    EventsLogger.logInvalidTargetId(mapRef.mapId, eventName, value);
                }
            }

            EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
        });
    }

    #getTarget(azmap: atlas.Map, event: IMapEventDef): atlas.layer.Layer | undefined {
        if (event.target === "layer") {
            return azmap.layers.getLayerById(event.targetId);
        }
    }

    #getCallback(mapRef: IMapReference, value: IMapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        switch (value.type as MapLayerEvent) {
            case MapLayerEvent.LayerAdded:
            case MapLayerEvent.LayerRemoved:
                callback = (callback: atlas.layer.Layer) => this.#notifyMapEventLayer(callback, mapRef, value)
                break;
            case MapLayerEvent.Click:
            case MapLayerEvent.ContextMenu:
            case MapLayerEvent.DblClick:
            case MapLayerEvent.MouseDown:
            case MapLayerEvent.MouseEnter:
            case MapLayerEvent.MouseLeave:
            case MapLayerEvent.MouseMove:
            case MapLayerEvent.MouseOut:
            case MapLayerEvent.MouseOver:
            case MapLayerEvent.MouseUp:
                callback = (callback: atlas.MapMouseEvent) => this.#notifyMapEventLayerMouse(callback, mapRef, value);
                break;
            case MapLayerEvent.TouchCancel:
            case MapLayerEvent.TouchEnd:
            case MapLayerEvent.TouchMove:
            case MapLayerEvent.TouchStart:
                callback = callback = (callback: atlas.MapTouchEvent) => this.#NotifyMapEventLayerTouch(callback, mapRef, value);
                break;
            case MapLayerEvent.Wheel:
                callback = (callback: atlas.MapMouseWheelEvent) => this.#notifyMapEventLayerWheel(callback, mapRef, value);
                break;
            default:
        }

        mapRef.eventsMap.addCallback(value, callback);

        return callback;
    }

    #notifyMapEventLayer = (callback: atlas.layer.Layer, mapRef: IMapReference, event: IMapEventDef) => {
        let result = EventsHelper.buildMapEventArgs(mapRef.mapId, event, EventsHelper.buildLayerEventPayload(callback), callback);
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEvent, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEvent, event.type);
    };

    #NotifyMapEventLayerTouch = (callback: atlas.MapTouchEvent, mapRef: IMapReference, event: IMapEventDef,) => {
        if (event.preventDefault)
            callback.preventDefault();
        let result = EventsHelper.buildMapEventArgs(mapRef.mapId, event, EventsHelper.buildTouchEventPayload(callback));
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEvent, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEvent, event.type);
    };

    #notifyMapEventLayerMouse = (callback: atlas.MapMouseEvent, mapRef: IMapReference, event: IMapEventDef) => {
        if (event.preventDefault)
            callback.preventDefault();
        let result = EventsHelper.buildMapEventArgs(mapRef.mapId, event, EventsHelper.buildMouseEventPayload(callback));
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEvent, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEvent, event.type);
    };

    #notifyMapEventLayerWheel = (callback: atlas.MapMouseWheelEvent, mapRef: IMapReference,  event: IMapEventDef) => {
        if (event.preventDefault)
            callback.preventDefault();
        let result = EventsHelper.buildMapEventArgs(mapRef.mapId, event, EventsHelper.buildWheelEventPayload(callback));
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEvent, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEvent, event.type);
    };
}

enum MapLayerEvent {
    LayerAdded = 'layeradded',
    LayerRemoved = 'layerremoved',

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

    TouchCancel = 'touchcancel',
    TouchEnd = 'touchend',
    TouchMove = 'touchmove',
    TouchStart = 'touchstart',

    Wheel = 'wheel',
}
