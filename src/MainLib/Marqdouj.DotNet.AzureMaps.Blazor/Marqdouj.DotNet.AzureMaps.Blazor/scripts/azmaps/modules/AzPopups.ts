import atlas from "azure-maps-control";
import { IJSInteropDef, IMapReference } from "../typings";
import { Helpers } from "../common/Helpers";
import { AzFactory } from "./AzFactory";
import { Logger, LogLevel } from "../common/Logger";

export class AzPopups {
    public static add(mapId: string, popups: IPopupDef[]): void {
        const mapRef = AzFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        popups ??= [];

        popups.forEach(popupDef => {
            let popup = new atlas.Popup(popupDef.options);
            (popup as any).jsInterop = Helpers.getJsInteropDef(popup);

            mapRef.map.popups.add(popup);
        });
    }

    public static remove(mapId: string, popups: IPopupDef[]): void {
        const mapRef = AzFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        popups.forEach(popupDef => {
            let popup = AzPopups.#doGetPopup(mapRef, popupDef.id);
            if (popup) {
                mapRef.map.popups.remove(popup);
            }
        });
    }

    static getPopup(mapRef: IMapReference, id: string): atlas.Popup {
        return AzPopups.#doGetPopup(mapRef, id);
    }

    static #doGetPopup(mapRef: IMapReference, id: string): atlas.Popup {
        const popups = mapRef.map.popups.getPopups();
        const popup = popups.findLast(value => AzPopups.#isInteropPopup(value, id));

        if (!popup) {
            Logger.logMessage(mapRef.mapId, LogLevel.Debug, `getPopup: popup not found where id = '${id}'`);
        }

        return popup;
    }

    static #isInteropPopup(obj: any, id?: string): obj is atlas.Popup {
        const jsInterop = Helpers.getJsInteropDef(obj);
        return obj instanceof atlas.Popup && jsInterop != undefined && jsInterop.id === id;
    }
}

interface IPopupDef  {
    id: string;
    jsInterop: IJSInteropDef;
    options: atlas.PopupOptions;
}
