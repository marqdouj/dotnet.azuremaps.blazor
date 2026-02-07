import atlas from "azure-maps-control";
import { Logger, LogLevel } from "../common/Logger";
import { AzFactory } from "./AzFactory";
import { IMapReference, IJSInteropDef, IMapEventDef } from "../typings";
import { Helpers } from "../common/Helpers";
import { AzEvents } from "./AzEvents";

export class AzMarkers {
    public static add(mapId: string, markers: IHtmlMarkerDef[], events?: IMapEventDef[]): void {
        const mapRef = AzFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        markers ??= [];

        markers.forEach(markerDef => {
            let options = { ...(markerDef as any).options };
            if (options.popup) {
                options.popup = new atlas.Popup(options.popup.options)
            }
            let marker = new atlas.HtmlMarker(options);
            (marker as any).jsInterop = markerDef.jsInterop;

            mapRef.map.markers.add(marker);

            if (events) {
                events.forEach(eventDef => {
                    eventDef.targetId = markerDef.id;
                });

                AzEvents.add(mapId, events);
            }

            if (markerDef.togglePopupOnClick) {
                mapRef.map.events.add('click', marker, () => {
                    marker.togglePopup();
                });
            }
        });
    }

    public static remove(mapId: string, markers: IHtmlMarkerDef[]): void {
        const mapRef = AzFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        markers.forEach(markerDef => {
            let marker = AzMarkers.#doGetMarker(mapRef, markerDef.id);
            if (marker) {
                mapRef.map.markers.remove(marker);
            }
        });
    }

    static getMarker(mapId: string, id: string): atlas.HtmlMarker {
        const mapRef = AzFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        return AzMarkers.#doGetMarker(mapRef, id);
    }

    static #doGetMarker(mapRef: IMapReference, id: string): atlas.HtmlMarker {
        const markers = mapRef.map.markers.getMarkers();
        const marker = markers.findLast(value => AzMarkers.#isInteropHtmlMarker(value, id));

        if (!marker) {
            Logger.logMessage(mapRef.mapId, LogLevel.Debug, `getMarker: marker not found where id = '${id}'`);
        }

        return marker;
    }

    static #isInteropHtmlMarker(obj: any, id: string): obj is atlas.HtmlMarker {
        const jsInterop = Helpers.getJsInteropDef(obj);
        return obj instanceof atlas.HtmlMarker && jsInterop != undefined && jsInterop.id === id;
    }
}

interface IHtmlMarkerDef {
    options: atlas.HtmlMarkerOptions;
    togglePopupOnClick: boolean;
    jsInterop: IJSInteropDef;
    id: string;
}
