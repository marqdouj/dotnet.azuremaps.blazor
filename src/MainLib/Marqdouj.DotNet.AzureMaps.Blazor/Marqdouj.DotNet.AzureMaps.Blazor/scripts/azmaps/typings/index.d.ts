import * as atlas from "azure-maps-control"
import { EventsMap } from "../modules/events/EventsMap";

type TProperties = { [key: string]: any };

interface IJSInteropDef {
    isMarqdouj: boolean;
    id: string;
}

interface IMapReference {
    readonly dotNetRef: any;
    readonly map: atlas.Map;
    readonly mapId: string;
    readonly eventsMap: EventsMap;
}

type TEventTarget = 'map' | 'datasource' | 'htmlmarker' | 'layer' | 'shape' | 'stylecontrol' | 'popup';

type MapEventArgs = {
    mapId: string;
    type: string;
    target: TEventTarget;
    targetId?: string;
    payload?: {
        jsInterop: IJSInteropDef;
        error?: any;
        mouse?: any;
    };
}

interface IMapEventDef {
    type: string;
    once?: boolean;
    target: TEventTarget;
    targetId?: string;
    targetSourceId?: string;
    preventDefault?: boolean;
}
