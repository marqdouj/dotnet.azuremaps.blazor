import * as anims from "azure-maps-animations"
import { IMapFeatureDef } from "./AzFeatures";
import { Helpers } from "../common/Helpers";
import { Logger, LogLevel } from "../common/Logger";
import { AzFactory } from "./AzFactory";
import { SourceHelper } from "./AzSources";

export class AzAnimations {
    static getEasingNames(mapId: string): string[] {
        if (AzAnimations.#animationsNotFound(mapId))
            return [];

        return anims.animations.getEasingNames();
    }

    static async animateShape(mapId: string, jsonOptions: string): Promise<void> {
        const options: AnimateShapeOptions = Helpers.parseJsonString(jsonOptions);

        switch (options.action) {
            case "SetCoordinates":
                await AzAnimations.#setCoordinates(mapId, options);
                break;
            default:
                Logger.logMessage(mapId, LogLevel.Error, "Animations.animateShape: action not supported.", options);
        }
    }

    static async #setCoordinates(mapId: string, options: AnimateShapeOptions): Promise<void> {
        if (AzAnimations.#animationsNotFound(mapId)) return;

        const map = AzFactory.getMap(mapId);
        if (!map) return;

        const eventName = "Animations.animateShape: ";

        const ds = map.sources.getById(options.dataSourceId);
        if (!ds) {
            Logger.logMessage(mapId, LogLevel.Error, `${eventName}DataSource not found.`, options);
            return;
        }

        const featureId = options.shape.id;

        if (SourceHelper.isDataSource(ds)) {
            let shape = ds.getShapeById(featureId);
            if (!shape) {
                Logger.logMessage(mapId, LogLevel.Error, `${eventName}Shape not found where shapeId = '${featureId}'.`, options);
                return;
            }

            anims.animations.setCoordinates(shape as any, options.shape.geometry.coordinates, options.animationOptions);
            shape.addProperty("heading", options.shape.properties["heading"]);
        } else {
            Logger.logMessage(mapId, LogLevel.Error, `${eventName}DataSource not found where id = '${options.dataSourceId}'.`, options);
        }
    }

    static #animationsNotFound(mapId: string): boolean {
        if (anims.animations) {
            return false;
        }
        else {
            Logger.logMessage(mapId, LogLevel.Trace, "Animations.setCoordinates: atlas.animations module not found.");
            return true;
        }
    }
}

type TAnimateAction = "SetCoordinates";

interface AnimateShapeOptions {
    action: TAnimateAction;
    shape: IMapFeatureDef;
    dataSourceId: string;
    animationOptions: anims.PlayableAnimationOptions;
}
