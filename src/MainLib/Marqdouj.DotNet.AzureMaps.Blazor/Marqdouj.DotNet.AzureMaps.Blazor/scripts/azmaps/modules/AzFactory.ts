import * as atlas from "azure-maps-control"
import { Logger, LogLevel } from "../common/Logger"
import { MapReference } from "./MapReference";
import { IMapEventDef, IMapReference, MapEventArgs } from "../typings";
import { Helpers } from "../common/Helpers";
import { AzEvents, EventNotification } from "./AzEvents"
import { EventsHelper } from "./events/EventsHelper";
import { AzControls, IMapControlDef } from "./AzControls";

export class AzFactory {
    static #azmaps: Map<string, MapReference> = new Map<string, MapReference>();
    static getAuthTokenCallback: atlas.getAuthTokenCallback;

    static createMap(params: ICreateMapParameters) {
        Logger.currentLevel = params?.logLevel ?? LogLevel.Information;

        const mapId = params.mapId;

        if (Helpers.isEmptyOrNull(mapId)) {
            Logger.logMessage("", LogLevel.Error, "Missing mapId.", params);
            return;
        }

        if (AzFactory.#azmaps.has(mapId)) {
            Logger.logMessage(mapId, LogLevel.Warn, "Map already exists.");
            return;
        }

        const options = AzFactory.#buildMapOptions(mapId, params.authOptions, params?.options);

        const azmap = new atlas.Map(mapId, options);
        const mapReference = new MapReference(params.dotNetRef, mapId, azmap);
        AzFactory.#azmaps.set(mapId, mapReference);
        Logger.logMessage(mapId, LogLevel.Debug, "was created.");

        AzControls.add(mapId, params.controls);
        AzFactory.#addEvents(mapReference, params.events);
    }

    static getMap(mapId: string): atlas.Map | undefined {
        const mapReference = AzFactory.getMapReference(mapId);
        return mapReference?.map;
    }

    static getMapReference(mapId: string): IMapReference | undefined {
        const mapReference = AzFactory.#azmaps.get(mapId);

        if (!mapReference) {
            Logger.logMessage(mapId, LogLevel.Debug, "MapManager was not found.");
        }

        return mapReference as IMapReference;
    }

    static removeMap(mapId: string): void {
        const mapReference = AzFactory.#azmaps.get(mapId);

        if (mapReference) {
            //GeolocationManager.clearWatch(mapId);

            if (AzFactory.#azmaps.delete(mapId)) {
                mapReference.clear();
                Logger.logMessage(mapId, LogLevel.Debug, "was removed");
            }
        }
    }

    static #buildMapOptions(mapId: string, authOptions: atlas.AuthenticationOptions, mapOptions?: any): TBuildMapOptions {
        let options: TBuildMapOptions = {};

        if (mapOptions) {
            //Camera and CameraBounds are mutually exclusive
            if (mapOptions.camera) {
                options = { ...options, ...mapOptions.camera };
            }
            else if (mapOptions.cameraBounds) {
                options = { ...options, ...mapOptions.cameraBounds };
            }

            if (mapOptions.service) {
                options = { ...options, ...mapOptions.service };
            }

            if (mapOptions.style) {
                options = { ...options, ...mapOptions.style };
            }
            if (mapOptions.userInteraction) {
                options = { ...options, ...mapOptions.userInteraction };
            }
        }

        options.authOptions = authOptions;
        const sasTokenUrl = (authOptions as any).sasTokenUrl;
        if (Helpers.isNotEmptyOrNull(sasTokenUrl)) {
            options.authOptions.getToken = function (resolve, reject, map) {
                fetch(sasTokenUrl).then(r => r.text()).then(token => resolve(token));
            }
        }
        else {
            options.authOptions.getToken = AzFactory.getAuthTokenCallback;
        }

        return options;
    }

    static #addEvents(mapReference: MapReference, events: any[]): void {
        const dotNetRef = mapReference.dotNetRef;
        const mapId = mapReference.mapId;
        const azmap = mapReference.map;

        if (!azmap) {
            Logger.logMessage(mapId, LogLevel.Error, "Cannot build events. Map not found.");
            return;
        }

        events ??= [];

        azmap.events.addOnce(MapEventCreate.Ready, event => {
            const errorDef: IMapEventDef = { target: "map", type: MapEventCreate.Error };
            const readyDef: IMapEventDef = { target: "map", type: MapEventCreate.Ready };

            azmap.events.add(MapEventCreate.Error, event => {
                const payload = { error: { message: event.error.message, name: event.error.name, stack: event.error.stack, cause: event.error.cause } };
                const args = EventsHelper.buildMapEventArgs(mapId, errorDef, payload);

                Logger.logMessage(mapId, LogLevel.Error, 'Map error', args);
                dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEventError, args);
            });

            AzEvents.add(mapId, events);

            const args = EventsHelper.buildMapEventArgs(mapId, readyDef);
            dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEventReady, args);
        });
    }
}

enum MapEventCreate {
    Error = 'error',
    Ready = 'ready',
}

type TBuildMapOptions = atlas.ServiceOptions & atlas.StyleOptions & atlas.UserInteractionOptions & (atlas.CameraOptions | atlas.CameraBoundsOptions);

interface ICreateMapParameters {
    dotNetRef: any;
    mapId: string;
    authOptions: atlas.AuthenticationOptions;
    options?: any;
    events?: IMapEventDef[];
    controls?: IMapControlDef[];
    logLevel?: number;
}
