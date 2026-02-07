import * as atlas from "azure-maps-control"
import { EventsLogger } from "./EventsLogger";
import { EventsHelper } from "./EventsHelper";
import { IMapEventDef, IMapReference } from "../../typings";
import { SourceHelper } from "../AzSources";
import { LogLevel } from "../../common/Logger";
import { Helpers } from "../../common/Helpers";
import { EventNotification } from "../AzEvents";

export class SourceEvents {
    add(mapRef: IMapReference, events: IMapEventDef[], source?: atlas.source.Source) {
        if (events.length == 0) return;

        if (source && !SourceHelper.isDataSource(source)) {
            EventsLogger.logMessage(mapRef.mapId, LogLevel.Error, `SourceEvents: Unsupported source type for adding events.`, source);
            return;
        }

        const dsEvents = this.#getEventDefs(events);
        if (dsEvents.length == 0) return;

        this.#addDataSourceEvents(mapRef, source as atlas.source.DataSource, dsEvents);
    }

    remove(mapRef: IMapReference, events: IMapEventDef[], source?: atlas.source.Source) {
        if (events.length == 0) return;

        if (source && !SourceHelper.isDataSource(source)) {
            EventsLogger.logMessage(mapRef.mapId, LogLevel.Error, `SourceEvents: Unsupported source type for removing events.`, source);
            return;
        }

        const dsEvents = this.#getEventDefs(events);
        if (dsEvents.length == 0) return;

        this.#removeDataSourceEvents(mapRef, source as atlas.source.DataSource, dsEvents);
    }

    // #region DataSource
    #getEventDefs(events: IMapEventDef[]) {
        return Object.values(events).filter(value => value.target === "datasource" && Helpers.isValueInEnum(MapEventDataSource, value.type));
    }

    #addDataSourceEvents(mapRef: IMapReference, ds: atlas.source.DataSource, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const azmap = mapRef.map;
        const eventName = "addDataSourceEvents";

        events.forEach((value) => {
            const target = ds ?? this.#getDataSourceTarget(mapRef, value);
            let wasAdded: boolean = false;

            if (ds) {
                value.targetId = ds.getId();
            }

            if (target) {
                const callback = this.#getCallback(mapRef, value, false);

                if (callback) {
                    if (value.once) {
                        azmap.events.addOnce(value.type as any, target, callback);
                    }
                    else {
                        azmap.events.add(value.type as any, target, callback);
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

    #removeDataSourceEvents(mapRef: IMapReference, ds: atlas.source.DataSource, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const azmap = mapRef.map;
        const eventName = "removeDataSourceEvents";

        events.forEach((value) => {
            const target = ds ?? this.#getDataSourceTarget(mapRef, value);
            let wasRemoved: boolean = false;

            if (ds) {
                value.targetId = ds.getId();
            }

            if (target) {
                const callback = this.#getCallback(mapRef, value, true);

                if (callback) {
                    azmap.events.remove(value.type as any, target, callback);
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

        switch (value.type.toLowerCase()) {
            case MapEventDataSource.DataSourceUpdated:
                callback = (callback: atlas.source.DataSource) => this.#NotifyMapDataSourceEvent_Updated(callback, mapRef, value);
                break;
            default:
                callback = (callback: atlas.Shape[]) => this.#NotifyMapDataSourceEvent(callback, mapRef, value);
        }

        mapRef.eventsMap.addCallback(value, callback);
        return callback;
    }

    #getDataSourceTarget(mapRef: IMapReference, event: IMapEventDef): atlas.source.DataSource | undefined {
        const target = SourceHelper.getSource(mapRef, event.targetId);
        if (target instanceof atlas.source.DataSource)
            return target;
    }

    #NotifyMapDataSourceEvent_Updated = (callback: atlas.source.DataSource, mapRef: IMapReference, event: IMapEventDef) => {
        let result = EventsHelper.buildMapEventArgs(mapRef.mapId, event, { dataSource: { id: callback.getId() } }, callback);
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEvent, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEvent, event.type);
    };

    #NotifyMapDataSourceEvent = (callback: atlas.Shape[], mapRef: IMapReference, event: IMapEventDef) => {
        let payload = { dataSource: { shapes: Helpers.buildShapeResults(callback) } };
        let result = EventsHelper.buildMapEventArgs(mapRef.mapId, event, payload);
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEvent, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEvent, event.type, result);
    };
    // #endregion
}

enum MapEventDataSource {
    DataSourceUpdated = 'datasourceupdated',
    DataAdded = 'dataadded',
    DataRemoved = 'dataremoved',
}