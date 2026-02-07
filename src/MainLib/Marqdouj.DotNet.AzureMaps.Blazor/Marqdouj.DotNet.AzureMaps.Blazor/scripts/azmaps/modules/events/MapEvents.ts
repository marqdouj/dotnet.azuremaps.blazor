import * as atlas from "azure-maps-control"
import { Helpers } from "../../common/Helpers";
import { IMapReference, IMapEventDef } from "../../typings";
import { EventsLogger } from "./EventsLogger";
import { EventsHelper } from "./EventsHelper";
import { EventNotification, MapEventMouse, MapEventTouch, MapEventWheel } from "../AzEvents";

export class MapEvents {
    add(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        this.#addMapEvents(mapRef, this.#getEventDefs(events));
    }

    remove(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        this.#removeMapEvents(mapRef, this.#getEventDefs(events));
    }

    #getEventDefs(events: IMapEventDef[]) {
        return Object.values(events).filter(value => value.target === "map" );
    }

    #addMapEvents(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        this.#addMapEventConfig(mapRef, events);
        this.#addMapEventData(mapRef, events);
        this.#addMapEventGeneral(mapRef, events);
        this.#addMapEventLayer(mapRef, events);
        this.#addMapEventMouse(mapRef, events);
        this.#addMapEventSource(mapRef, events);
        this.#addMapEventStyle(mapRef, events);
        this.#addMapEventTouch(mapRef, events);
        this.#addMapEventWheel(mapRef, events);
    }

    #removeMapEvents(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        this.#removeMapEventConfig(mapRef, events);
        this.#removeMapEventData(mapRef, events);
        this.#removeMapEventGeneral(mapRef, events);
        this.#removeMapEventLayer(mapRef, events);
        this.#removeMapEventMouse(mapRef, events);
        this.#removeMapEventSource(mapRef, events);
        this.#removeMapEventStyle(mapRef, events);
        this.#removeMapEventTouch(mapRef, events);
        this.#removeMapEventWheel(mapRef, events);
    }

    // #region Config
    #addMapEventConfig(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addMapEventConfig";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventConfig, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallbackConfig(mapRef, value, false);

            if (callback) {
                if (value.once) {
                    EventsLogger.logOnceNotSupported(mapRef.mapId, eventName, value);
                }
                else {
                    mapRef.map.events.add(value.type as MapEventConfig, callback);
                    wasAdded = true;
                }
            }
            EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventConfig(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeMapEventConfig";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventConfig, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            const callback = this.#getCallbackConfig(mapRef, value, true);

            if (callback) {
                mapRef.map.events.remove(value.type, callback);
                wasRemoved = true;
            }
            EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackConfig(mapRef: IMapReference, event: IMapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(event, removing);

        if (callback) {
            return callback;
        }

        callback = (config: atlas.MapConfiguration) => this.#notifyNotifyMapEventConfig(config, mapRef, event);

        mapRef.eventsMap.addCallback(event, callback);

        return callback;
    }

    #notifyNotifyMapEventConfig = (callback: atlas.MapConfiguration, mapRef: IMapReference, event: IMapEventDef) => {
        const result = EventsHelper.buildMapEventArgs(mapRef.mapId, event, { config: { ...callback } });
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEvent, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEvent, event.type);
    };
    // #endregion

    // #region Data
    #addMapEventData(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addMapEventData";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventData, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallbackData(mapRef, value, false);

            if (callback) {
                if (value.once) {
                    mapRef.map.events.addOnce(value.type as MapEventData, callback);
                }
                else {
                    mapRef.map.events.add(value.type as MapEventData, callback);
                }
                wasAdded = true;
            }

            EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventData(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeMapEventData";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventData, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            const callback = this.#getCallbackData(mapRef, value, true);

            if (callback) {
                mapRef.map.events.remove(value.type, callback);
                wasRemoved = true;
            }

            EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackData(mapRef: IMapReference, event: IMapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(event, removing);

        if (callback) {
            return callback;
        }

        callback = (dataEvent: atlas.MapDataEvent) => this.#notifyMapEventData(dataEvent, mapRef, event);

        mapRef.eventsMap.addCallback(event, callback);

        return callback;
    }

    #notifyMapEventData = (callback: atlas.MapDataEvent, mapRef: IMapReference, event: IMapEventDef) => {
        let result = EventsHelper.buildMapEventArgs(mapRef.mapId, event, this.#buildMapDataEventPayload(callback));
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEvent, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEvent, event.type);
    };

    #buildMapDataEventPayload(dataEvent: atlas.MapDataEvent) {
        const payload = {
            dataType: dataEvent.dataType,
            isSourceLoaded: dataEvent.isSourceLoaded,
            source: dataEvent.source?.getId(),
            sourceDataType: dataEvent.sourceDataType,
            tile: dataEvent.tile
        };

        return { data: payload };
    }
    // #endregion

    // #region General
    #addMapEventGeneral(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addMapEventGeneral";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventGeneral, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallbackGeneral(mapRef, value, false);

            if (callback) {
                if (value.once) {
                    mapRef.map.events.addOnce(value.type as MapEventGeneral, callback);
                }
                else {
                    mapRef.map.events.add(value.type as MapEventGeneral, callback);
                }
                wasAdded = true;
            }

            EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventGeneral(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeMapEventGeneral";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventGeneral, value.type)).forEach((value) => {
            let wasRemoved = false;
            const callback = this.#getCallbackGeneral(mapRef, value, true);

            if (callback) {
                mapRef.map.events.remove(value.type, callback);
                wasRemoved = true;
            }

            EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackGeneral(mapRef: IMapReference, event: IMapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(event, removing);

        if (callback) {
            return callback;
        }

        callback = (callback: atlas.MapEvent) => { this.#notifyMapEventGeneral(callback, mapRef, event); };

        mapRef.eventsMap.addCallback(event, callback);

        return callback;
    }

    #notifyMapEventGeneral = (callback: atlas.MapEvent, mapRef: IMapReference, event: IMapEventDef) => {
        let result = EventsHelper.buildMapEventArgs(mapRef.mapId, event);
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEvent, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEvent, event.type);
    };
    // #endregion

    // #region Layer
    #addMapEventLayer(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addMapEventLayer";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventLayer, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallbackLayer(mapRef, value, false);

            if (callback) {
                if (value.once) {
                    mapRef.map.events.addOnce(value.type as MapEventLayer, callback);
                }
                else {
                    mapRef.map.events.add(value.type as MapEventLayer, callback);
                }
                wasAdded = true;
            }

            EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventLayer(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeMapEventLayer";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventLayer, value.type)).forEach((value) => {
            let wasRemoved = false;
            const callback = this.#getCallbackLayer(mapRef, value, true);

            if (callback) {
                mapRef.map.events.remove(value.type, callback);
                wasRemoved = true;
            }

            EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackLayer(mapRef: IMapReference, event: IMapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(event, removing);

        if (callback) {
            return callback;
        }

        callback = (callback: atlas.layer.Layer) => { this.#notifyMapEventLayer(callback, mapRef, event); };

        mapRef.eventsMap.addCallback(event, callback);

        return callback;
    }

    #notifyMapEventLayer = (callback: atlas.layer.Layer, mapRef: IMapReference, event: IMapEventDef) => {
        let result = EventsHelper.buildMapEventArgs(mapRef.mapId, event, EventsHelper.buildLayerEventPayload(callback), callback);
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEvent, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEvent, event.type);
    };
    // #endregion

    // #region Mouse
    #addMapEventMouse(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addMapEventMouse";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventMouse, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            let callback = this.#getCallBackMouse(mapRef, value, false);

            if (callback) {
                if (value.once) {
                    mapRef.map.events.addOnce(value.type as MapEventMouse, callback);
                }
                else {
                    mapRef.map.events.add(value.type as MapEventMouse, callback);
                }
                wasAdded = true;
            }

            EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventMouse(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeMapEventMouse";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventMouse, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            let callback = this.#getCallBackMouse(mapRef, value, true);

            if (callback) {
                mapRef.map.events.remove(value.type, callback);
                wasRemoved = true;
            }

            EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallBackMouse(mapRef: IMapReference, event: IMapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(event, removing);

        if (callback) {
            return callback;
        }

        callback = (callback: atlas.MapMouseEvent) => this.#notifyMapEventMouse(callback, mapRef, event);

        mapRef.eventsMap.addCallback(event, callback);
        return callback;
    }

    #notifyMapEventMouse = (callback: atlas.MapMouseEvent, mapRef: IMapReference, event: IMapEventDef) => {
        if (event.preventDefault)
            callback.preventDefault();
        let result = EventsHelper.buildMapEventArgs(mapRef.mapId, event, EventsHelper.buildMouseEventPayload(callback));
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEvent, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEvent, event.type);
    };

    // #endregion

    // #region Source
    #addMapEventSource(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addMapEventSource";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventSource, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallbackSource(mapRef, value, false);

            if (callback) {
                if (value.once) {
                    mapRef.map.events.addOnce(value.type as MapEventSource, callback);
                }
                else {
                    mapRef.map.events.add(value.type as MapEventSource, callback);
                }
                wasAdded = true;
            }

            EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventSource(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeMapEventSource";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventSource, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            const callback = this.#getCallbackSource(mapRef, value, true);

            if (callback) {
                mapRef.map.events.remove(value.type, callback);
                wasRemoved = true;
            }

            EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackSource(mapRef: IMapReference, event: IMapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(event, removing);

        if (callback) {
            return callback;
        }

        callback = (source: atlas.source.Source) => this.#notifyMapEventSource(source, mapRef, event);

        mapRef.eventsMap.addCallback(event, callback);

        return callback;
    }

    #notifyMapEventSource(callback: atlas.source.Source, mapRef: IMapReference, event: IMapEventDef) {
        let payload = { source: { id: callback.getId() } };
        let result = EventsHelper.buildMapEventArgs(mapRef.mapId, event, { source: { id: callback.getId() } }, callback);
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEvent, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEvent, event.type);
    }
    // #endregion

    // #region Style
    #addMapEventStyle(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addMapEventStyle";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventStyle, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallbackStyle(mapRef, value, false);

            if (callback) {
                if (value.once) {
                    mapRef.map.events.addOnce(value.type as any, callback);
                }
                else {
                    mapRef.map.events.add(value.type as any, callback);
                }
                wasAdded = true;
            }

            EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventStyle(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeMapEventStyle";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventStyle, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            const callback = this.#getCallbackStyle(mapRef, value, true);

            if (callback) {
                mapRef.map.events.remove(value.type, callback);
                wasRemoved = true;
            }

            EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackStyle(mapRef: IMapReference, event: IMapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(event, removing);

        if (callback) {
            return callback;
        }

        switch (event.type.toLowerCase()) {
            case MapEventStyle.StyleChanged:
                callback = (source: atlas.StyleChangedEvent) => this.#notifyMapEventStyle(source.style, mapRef, event);
                break;
            case MapEventStyle.StyleImageMissing:
                callback = (style: string) => this.#notifyMapEventStyle(style, mapRef, event);
                break;
            default:
        }

        mapRef.eventsMap.addCallback(event, callback);

        return callback;
    }

    #notifyMapEventStyle(style: string, mapRef: IMapReference, event: IMapEventDef) {
        let result = EventsHelper.buildMapEventArgs(mapRef.mapId, event, { style: { style: style } });
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEvent, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEvent, event.type);
    }
    // #endregion

    // #region Touch
    #addMapEventTouch(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addMapEventTouch";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventTouch, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallbackTouch(mapRef, value, false);

            if (callback) {
                if (value.once) {
                    mapRef.map.events.addOnce(value.type as MapEventTouch, callback);
                }
                else {
                    mapRef.map.events.add(value.type as MapEventTouch, callback);
                }
                wasAdded = true;
            }

            EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventTouch(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeMapEventTouch";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventTouch, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            const callback = this.#getCallbackTouch(mapRef, value, true);

            if (callback) {
                mapRef.map.events.remove(value.type, callback);
                wasRemoved = true;
            }

            EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackTouch(mapRef: IMapReference, event: IMapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(event, removing);

        if (callback) {
            return callback;
        }

        callback = (callback: atlas.MapTouchEvent) => this.#notifyMapEventTouch(callback, mapRef, event);

        mapRef.eventsMap.addCallback(event, callback);

        return callback;
    }

    #notifyMapEventTouch = (callback: atlas.MapTouchEvent, mapRef: IMapReference, event: IMapEventDef) => {
        if (event.preventDefault)
            callback.preventDefault();
        let result = EventsHelper.buildMapEventArgs(mapRef.mapId, event, EventsHelper.buildTouchEventPayload(callback));
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEvent, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEvent, event.type);
    };

    // #endregion

    // #region Wheel
    #addMapEventWheel(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addMapEventWheel";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventWheel, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            let callback = this.#getCallbackWheel(mapRef, value, false);

            if (callback) {
                if (value.once) {
                    mapRef.map.events.addOnce(value.type as MapEventWheel, callback);
                }
                else {
                    mapRef.map.events.add(value.type as MapEventWheel, callback);
                }
                wasAdded = true;
            }

            EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventWheel(mapRef: IMapReference, events: IMapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeMapEventWheel";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventWheel, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            let callback = this.#getCallbackWheel(mapRef, value, true);

            if (callback) {
                mapRef.map.events.remove(value.type, callback);
                wasRemoved = true;
            }

            EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackWheel(mapRef: IMapReference, event: IMapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(event, removing);

        if (callback) {
            return callback;
        }

        callback = (callback: atlas.MapMouseWheelEvent) => this.#notifyMapEventWheel(callback, mapRef, event);

        mapRef.eventsMap.addCallback(event, callback);

        return callback;
    }

    #notifyMapEventWheel = (callback: atlas.MapMouseWheelEvent, mapRef: IMapReference, event: IMapEventDef) => {
        if (event.preventDefault)
            callback.preventDefault();
        let result = EventsHelper.buildMapEventArgs(mapRef.mapId, event, EventsHelper.buildWheelEventPayload(callback));
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEvent, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEvent, event.type);
    };

    // #endregion
}

enum MapEventConfig {
    MapConfigChanged = 'mapconfigurationchanged',
}

enum MapEventData {
    Data = 'data',
    SourceData = 'sourcedata',
    StyleData = 'styledata',
}

enum MapEventGeneral {
    BoxZoomEnd = 'boxzoomend',
    BoxZoomStart = 'boxzoomstart',
    Drag = 'drag',
    DragEnd = 'dragend',
    DragStart = 'dragstart',
    Idle = 'idle',
    Load = 'load',
    Move = 'move',
    MoveEnd = 'moveend',
    MoveStart = 'movestart',
    Pitch = 'pitch',
    PitchEnd = 'pitchend',
    PitchStart = 'pitchstart',
    Render = 'render',
    Resize = 'resize',
    Rotate = 'rotate',
    RotateEnd = 'rotateend',
    RotateStart = 'rotatestart',
    TokenAcquired = 'tokenacquired',
    Zoom = 'zoom',
    ZoomEnd = 'zoomend',
    ZoomStart = 'zoomstart'
}

enum MapEventLayer {
    LayerAdded = 'layeradded',
    LayerRemoved = 'layerremoved'
}

enum MapEventSource {
    SourceAdded = 'sourceadded',
    SourceRemoved = 'sourceremoved'
}

enum MapEventStyle {
    StyleChanged = 'stylechanged',
    StyleImageMissing = 'styleimagemissing',
}
