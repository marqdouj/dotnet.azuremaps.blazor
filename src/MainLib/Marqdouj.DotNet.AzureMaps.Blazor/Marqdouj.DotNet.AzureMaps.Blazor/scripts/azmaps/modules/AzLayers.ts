import atlas from "azure-maps-control";
import { Logger, LogLevel } from "../common/Logger";
import { AzFactory } from "./AzFactory";
import { IMapReference, IJSInteropDef, IMapEventDef } from "../typings";
import { Helpers } from "../common/Helpers";
import { AzSources, DataSourceDef, SourceHelper } from "./AzSources";
import { AzEvents } from "./AzEvents";

export class AzLayers {
    public static addGroups(mapId: string, layers: IMapLayerEventsGroup[]) {
        const mapRef = AzFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        layers ??= [];

        layers.forEach((def) => {
            AzLayers.#doAddLayer(mapRef, def.layer, def.events);
        });
    }

    public static add(mapId: string, layers: IMapLayerDef[], events?: IMapEventDef[]): void {
        const mapRef = AzFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        layers ??= [];

        layers.forEach((def) => {
            AzLayers.#doAddLayer(mapRef, def, events);
        });
    }

    static #doAddLayer(mapRef: IMapReference, def: IMapLayerDef, events?: IMapEventDef[]): void {
        const eventName = "addLayer";

        if (Helpers.isEmptyOrNull(def.type)) {
            Logger.logMessage(mapRef.mapId, LogLevel.Error, `${eventName}: layer type is missing`, def);
            return;
        }

        const layerId = def.id;

        if (Helpers.isEmptyOrNull(layerId)) {
            Logger.logMessage(mapRef.mapId, LogLevel.Error, `${eventName}: layer Id is missing`, def);
            return;
        }

        const lyr = mapRef.map.layers.getLayerById(layerId);
        if (lyr) {
            Logger.logMessage(mapRef.mapId, LogLevel.Error, `${eventName}: layer already exists where layer ID=${layerId}`, def);
            return;
        }

        let src: atlas.source.Source;
        let dsDef: DataSourceDef = def.dataSource;
        const dsId = dsDef.id;

        if (dsDef && Helpers.isNotEmptyOrNull(dsId)) {
            src = SourceHelper.getSource(mapRef, dsId, false);

            if (!src) {
                AzSources.add(mapRef.mapId, [dsDef], events);
                src = SourceHelper.getSource(mapRef, dsId);
                if (!src) {
                    Logger.logMessage(mapRef.mapId, LogLevel.Error, `${eventName}: Unable to create datasource.`, dsDef);
                }
            }
        }

        let layer: atlas.layer.Layer;
        const layerOptions = (def.options || {}) as any;

        switch (def.type) {
            case 'Bubble':
                layer = new atlas.layer.BubbleLayer(src, layerId, layerOptions);
                break;
            case 'HeatMap':
                layer = new atlas.layer.HeatMapLayer(src, layerId, layerOptions);
                break;
            case 'Image':
                layer = new atlas.layer.ImageLayer(layerOptions, layerId);
                break;
            case 'Line':
                layer = new atlas.layer.LineLayer(src, layerId, layerOptions);
                break;
            case 'Polygon':
                layer = new atlas.layer.PolygonLayer(src, layerId, layerOptions);
                break;
            case 'PolygonExtrusion':
                layer = new atlas.layer.PolygonExtrusionLayer(src, layerId, layerOptions);
                break;
            case 'Symbol':
                AzLayers.#resolveSymbolLayerOptions(mapRef.mapId, layerOptions);
                layer = new atlas.layer.SymbolLayer(src, layerId, layerOptions);
                break;
            case 'Tile':
                layer = new atlas.layer.TileLayer(layerOptions, layerId);
                break;
            default:
                break;
        }

        let wasAdded = false;
        if (layer) {
            (layer as any).jsInterop = def.jsInterop;

            if (events) {
                AzEvents.layers.add(mapRef, events, layer);
            }

            mapRef.map.layers.add(layer, def.before);
            wasAdded = true;
        }

        if (wasAdded) {
            Logger.logMessage(mapRef.mapId, LogLevel.Trace, `${eventName}: layer added:`, def);
        } else {
            Logger.logMessage(mapRef.mapId, LogLevel.Error, `${eventName}: layer type '${def.type}' is not supported.`, def);
        }
    }

    public static remove(mapId: string, layers: IMapLayerDef[]): void {
        let idList: string[] = [];

        layers.forEach((layerDef) => {
            idList.push(layerDef.id);
        });

        AzLayers.removeById(mapId, idList);

        idList = [];
        layers.forEach((layerDef) => {
            if (layerDef.dataSource && Helpers.isNotEmptyOrNull(layerDef.dataSource.id)) {
                idList.push(layerDef.dataSource.id);
            }
        });

        AzSources.removeById(mapId, idList);
    }

    public static removeById(mapId: string, layerIds: string[]): void {
        const mapRef = AzFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        const eventName = "removeLayer";

        layerIds.forEach((id) => {
            const lyr = mapRef.map.layers.getLayerById(id);
            if (lyr) {
                mapRef.map.layers.remove(lyr);
                Logger.logMessage(mapId, LogLevel.Debug, `${eventName}: layer with id '${id}' was removed.`);
            }
            else {
                Logger.logMessage(mapId, LogLevel.Warn, `${eventName}: layer with id '${id}' was not found.`);
            }
        });
    }

    public static getOptions(mapId: string, id: string) {
        const mapRef = AzFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        const lyr = mapRef.map.layers.getLayerById(id);

        if (!lyr) {
            Logger.logMessage(mapId, LogLevel.Error, `AzLayers.getOptions: layer does not exist where layer ID=${id}`);
            return;
        }

        let options = lyr.getOptions();
        options.source = null; //get rid of circular references

        return options;
    }

    public static setOptions(mapId: string, layerDef: IMapLayerDef): void {
        const mapRef = AzFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        const lyr = mapRef.map.layers.getLayerById(layerDef.id);

        if (!lyr) {
            Logger.logMessage(mapId, LogLevel.Error, `AzLayers.setOptions: layer does not exist where layer ID=${layerDef.id}`, layerDef);
            return;
        }

        let layerOptions = (layerDef.options || {}) as any;

        switch (layerDef.type) {
            case "Symbol":
                AzLayers.#resolveSymbolLayerOptions(mapId, layerOptions);
                break;
            default:
        }

        const getOptions = lyr.getOptions();
        const options = { ...getOptions, ...layerOptions };

        Logger.logMessage(mapId, LogLevel.Trace, "Layers.setOptions", layerOptions, options);
        lyr.setOptions(options);
    }

    static #resolveSymbolLayerOptions(mapId: string, layerOptions: atlas.SymbolLayerOptions) {
        const iconOptions = layerOptions.iconOptions;

        if (!iconOptions) return;

        const imageId = iconOptions.imageId;
        if (Helpers.isNotEmptyOrNull(imageId)) {
            iconOptions.image = imageId;
        }

        const rotationSpec = iconOptions.rotationSpecification
        if (rotationSpec) {
            iconOptions.rotation = rotationSpec;
        }

        Logger.logMessage(mapId, LogLevel.Trace, "resolveSymbolLayerOptions:", layerOptions);
    }
}

interface IMapLayerDef {
    id: string;
    jsInterop: IJSInteropDef;
    type: 'Bubble' | 'HeatMap' | 'Image' | 'Line' | 'Polygon' | 'PolygonExtrusion' | 'Symbol' | 'Tile';
    dataSource: DataSourceDef;
    before?: string;
    options?:
    atlas.BubbleLayerOptions |
    atlas.HeatMapLayerOptions |
    atlas.ImageLayerOptions |
    atlas.LineLayerOptions |
    atlas.PolygonLayerOptions |
    atlas.PolygonExtrusionLayerOptions |
    atlas.SymbolLayerOptions |
    atlas.TileLayerOptions |
    atlas.WebGLLayerOptions;
}

interface IMapLayerEventsGroup {
    layer: IMapLayerDef;
    events?: IMapEventDef[];
}