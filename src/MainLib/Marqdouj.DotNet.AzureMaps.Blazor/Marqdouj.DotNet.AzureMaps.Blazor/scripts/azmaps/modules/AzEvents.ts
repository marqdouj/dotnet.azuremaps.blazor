import { Logger, LogLevel } from "../common/Logger";
import { AzFactory } from "./AzFactory";
import { LayerEvents } from "./events/LayerEvents";
import { MarkerEvents } from "./events/MarkerEvents";
import { MapEvents } from "./events/MapEvents";
import { PopupEvents } from "./events/PopupEvents";
import { SourceEvents } from "./events/SourceEvents";
import { StyleControlEvents } from "./events/StyleControlEvents";
import { IMapEventDef } from "../typings";
import { ShapeEvents } from "./events/ShapeEvents";

export class AzEvents {
    static readonly maps: MapEvents = new MapEvents();
    static readonly layers: LayerEvents = new LayerEvents()
    static readonly markers: MarkerEvents = new MarkerEvents();
    static readonly popups: PopupEvents = new PopupEvents();
    static readonly shapes: ShapeEvents = new ShapeEvents();
    static readonly sources: SourceEvents = new SourceEvents();
    static readonly styleControls: StyleControlEvents = new StyleControlEvents();


    static add(mapId: string, events: IMapEventDef[]) {
        const mapRef = AzFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        Logger.logMessage(mapId, LogLevel.Trace, "AzEvents:add", events);

        events ??= [];
        AzEvents.maps.add(mapRef, events);
        AzEvents.layers.add(mapRef, events);
        AzEvents.markers.add(mapRef, events);
        AzEvents.popups.add(mapRef, events);
        AzEvents.shapes.add(mapRef, events);
        AzEvents.sources.add(mapRef, events);
        AzEvents.styleControls.add(mapRef, events);
    }

    public static remove(mapId: string, events: IMapEventDef[]): void {
        const mapRef = AzFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        AzEvents.layers.remove(mapRef, events);
        AzEvents.maps.remove(mapRef, events);
        AzEvents.markers.remove(mapRef, events);
        AzEvents.popups.remove(mapRef, events);
        AzEvents.shapes.remove(mapRef, events);
        AzEvents.sources.remove(mapRef, events);
        AzEvents.styleControls.remove(mapRef, events);
    }
}

export enum EventNotification {
    NotifyMapEventError = 'NotifyMapEventError',
    NotifyMapEventReady = 'NotifyMapEventReady',
    NotifyMapEvent = 'NotifyMapEvent',
    NotifyGeolocationWatch = 'NotifyGeolocationWatch',
}

export enum MapEventMouse {
    Click = 'click',
    ContextMenu = 'contextmenu',
    DblClick = 'dblclick',
    MouseDown = 'mousedown',
    MouseMove = 'mousemove',
    MouseOut = 'mouseout',
    MouseOver = 'mouseover',
    MouseUp = 'mouseup',
}

export enum MapEventTouch {
    TouchCancel = 'touchcancel',
    TouchEnd = 'touchend',
    TouchMove = 'touchmove',
    TouchStart = 'touchstart'
}

export enum MapEventWheel {
    Wheel = 'wheel',
}
