import atlas from "azure-maps-control";
import { EventsHelper } from "./EventsHelper";
import { EventsLogger } from "./EventsLogger"
import { IMapEventDef, IMapReference } from "../../typings";
import { Helpers } from "../../common/Helpers";
import { EventNotification } from "../AzEvents";
import { AzControls } from "../AzControls";

export class StyleControlEvents {
    add(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        this.#addEvents(mapRef, this.#getEventDefs(events));
    }

    remove(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        this.#removeEvents(mapRef, this.#getEventDefs(events));
    }

    #getEventDefs(events: IMapEventDef[]) {
        return Object.values(events).filter(value => value.target === "stylecontrol" && Helpers.isValueInEnum(StyleControlEvent, value.type));
    }

    #addEvents(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addStyleControlEvents";

        events.forEach((value) => {
            const target = AzControls.getControl(mapRef.mapId, value.targetId);

            if (target instanceof atlas.control.StyleControl) {
                let wasAdded: boolean = false;
                const callback = this.#getCallback(mapRef, value, false);

                if (callback) {
                    if (value.once) {
                        mapRef.map.events.addOnce(value.type as StyleControlEvent, target, callback);
                    }
                    else {
                        mapRef.map.events.add(value.type as StyleControlEvent, target, callback);
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

    #removeEvents(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeStyleControlEvents";

        events.forEach((value) => {
            const target = AzControls.getControl(mapRef.mapId, value.targetId);

            if (target instanceof atlas.control.StyleControl) {
                let wasRemoved: boolean = false;
                const callback = this.#getCallback(mapRef, value, true);

                if (callback) {
                    mapRef.map.events.remove(value.type, target, callback);
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

        callback = (style: string) => this.#notifyStyleControlEvent(style, mapRef, value);

        mapRef.eventsMap.addCallback(value, callback);
        return callback;
    }

    #notifyStyleControlEvent = (style: string, mapRef: IMapReference, event: IMapEventDef) => {
        let result = EventsHelper.buildMapEventArgs(mapRef.mapId, event, { styleControl: { style: style } });
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEvent, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEvent, event.type);
    };
}

enum StyleControlEvent {
    StyleSelected = 'styleselected',
}

