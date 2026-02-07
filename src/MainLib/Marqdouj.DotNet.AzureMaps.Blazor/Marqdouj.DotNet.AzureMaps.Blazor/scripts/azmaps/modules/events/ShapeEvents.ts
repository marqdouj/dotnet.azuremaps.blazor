import atlas from "azure-maps-control";
import { Helpers } from "../../common/Helpers";
import { EventsLogger } from "./EventsLogger";
import { EventsHelper } from "./EventsHelper";
import { IMapReference, IMapEventDef } from "../../typings";
import { EventNotification } from "../AzEvents";
import { SourceHelper } from "../AzSources";

export class ShapeEvents {
    add(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        this.#addShapeEvents(mapRef, this.#getEventDefs(events));
    }

    remove(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        this.#removeShapeEvents(mapRef, this.#getEventDefs(events));
    }

    #getEventDefs(events: IMapEventDef[]) {
        return Object.values(events).filter(value => value.target === "shape" && Helpers.isValueInEnum(MapShapeEvent, value.type));
    }

    #addShapeEvents(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addShapeEvents";

        events.forEach((value) => {
            const target = this.#getTarget(mapRef, value);
            let wasAdded: boolean = false;

            if (target) {
                const callback = this.#getCallback(mapRef, value, false);

                if (callback) {
                    if (value.once) {
                        mapRef.map.events.addOnce(value.type as MapShapeEvent, target, callback);
                    }
                    else {
                        mapRef.map.events.add(value.type as MapShapeEvent, target, callback);
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

    #removeShapeEvents(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeShapeEvents";

        events.forEach((value) => {
            const target = this.#getTarget(mapRef, value);
            let wasRemoved: boolean = false;

            if (target) {
                const callback = this.#getCallback(mapRef, value, true);

                if (callback) {
                    mapRef.map.events.remove(value.type as MapShapeEvent, target, callback);
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

        callback = (callback: atlas.Shape) => this.#NotifyMapShapeEvent(callback, mapRef, value);

        mapRef.eventsMap.addCallback(value, callback);
        return callback;
    }

    #getTarget(mapRef: IMapReference, event: IMapEventDef) {
        const ds = SourceHelper.getSource(mapRef, event.targetSourceId);
        if (SourceHelper.isDataSource(ds)) {
            const target = ds.getShapeById(event.targetId);
            return target;
        }
    }

    #NotifyMapShapeEvent = (callback: atlas.Shape, mapRef: IMapReference, event: IMapEventDef) => {
        let result = EventsHelper.buildMapEventArgs(mapRef.mapId, event, Helpers.getShapeResult(callback), callback);
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEvent, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEvent, event.type);
    };
}

enum MapShapeEvent {
    ShapeChanged = 'shapechanged',
}
