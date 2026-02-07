import * as atlas from "azure-maps-control"
import { Helpers } from "../common/Helpers";
import { AzFactory } from "./AzFactory";
import { IJSInteropDef } from "../typings";
import { Logger, LogLevel } from "../common/Logger";

export class AzControls {
    public static add(mapId: string, mapControls: IMapControlDef[]): void {
        const map = AzFactory.getMap(mapId);
        if (!map) return;

        mapControls ??= [];

        mapControls.forEach(mapControl => {
            let control: atlas.Control = null;

            switch (mapControl.type.toLowerCase()) {
                case "compass":
                    control = new atlas.control.CompassControl(mapControl.options as atlas.CompassControlOptions);
                    break;
                case "fullscreen":
                    control = new atlas.control.FullscreenControl(mapControl.options as atlas.FullscreenControlOptions);
                    break;
                case "pitch":
                    control = new atlas.control.PitchControl(mapControl.options as atlas.PitchControlOptions);
                    break;
                case "scale":
                    control = new atlas.control.ScaleControl(mapControl.options as atlas.ScaleControlOptions);
                    break;
                case "style":
                    control = new atlas.control.StyleControl(mapControl.options as atlas.StyleControlOptions);
                    break;
                case "traffic":
                    control = new atlas.control.TrafficControl(mapControl.options as atlas.TrafficControlOptions);
                    break;
                case "trafficlegend":
                    control = new atlas.control.TrafficLegendControl();
                    break;
                case "zoom":
                    control = new atlas.control.ZoomControl(mapControl.options as atlas.ZoomControlOptions);
                    break;
                default:
                    Logger.logMessage(mapId, LogLevel.Warn, `addControls: control type '${mapControl.type}' is not supported.`);
                    break;
            }

            if (control) {
                (control as any).jsInterop = mapControl.jsInterop;
                map.controls.add(control, mapControl.controlOptions);
            }
        });
    }

    public static remove(mapId: string, mapControls: IMapControlDef[]): void {
        const map = AzFactory.getMap(mapId);
        if (!map) return;

        const controls = map.controls.getControls();
        mapControls.forEach(item => {
            let control = AzControls.#doGetControl(controls, mapId, item.id);
            if (control) {
                map.controls.remove(control);
            }
        });
    }

    static getControl(mapId: string, id: string): atlas.Control {
        const map = AzFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const controls = map.controls.getControls();
        return AzControls.#doGetControl(controls, mapId, id);
    }

    static #doGetControl(controls: atlas.Control[], mapId: string, id: string): atlas.Control {
        const control = controls.findLast(value => AzControls.#isInteropControl(value, id));

        if (!control) {
            Logger.logMessage(mapId, LogLevel.Debug, `getControl: control not found where id = '${id}'`);
        }

        return control;
    }

    static #isInteropControl(obj: any, id?: string): obj is atlas.Control {
        const jsInterop = Helpers.getJsInteropDef(obj);
        return obj && jsInterop != undefined && jsInterop.id === id;
    }
}

export interface IMapControlDef {
    type: 'compass' | 'fullscreen' | 'pitch' | 'scale' | 'style' | 'traffic' | 'trafficLegend' | 'zoom';
    position: atlas.ControlPosition;
    options: atlas.CompassControlOptions
    | atlas.FullscreenControlOptions
    | atlas.PitchControlOptions
    | atlas.ScaleControlOptions
    | atlas.StyleControlOptions
    | atlas.ZoomControlOptions;
    controlOptions: atlas.ControlOptions;
    jsInterop: IJSInteropDef;
    id: string;
}