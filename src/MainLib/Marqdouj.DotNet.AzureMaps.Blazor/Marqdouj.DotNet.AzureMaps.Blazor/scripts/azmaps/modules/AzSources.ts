import atlas from "azure-maps-control";
import { IJSInteropDef, IMapEventDef, IMapReference } from "../typings";
import { Logger, LogLevel } from "../common/Logger";
import { Helpers } from "../common/Helpers";
import { AzFactory } from "./AzFactory";
import { AzEvents } from "./AzEvents";

export class AzSources {
    // #region Add
    public static add(mapId: string, sources: SourceDef[], events?: IMapEventDef[]): void {
        const mapRef = AzFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        sources ??= [];

        sources.forEach((sourceDef) => {
            AzSources.#doAdd(mapRef, sourceDef, events);
        });
    }

    static #doAdd(mapRef: IMapReference, def: SourceDef, events?: IMapEventDef[]): void {
        switch (def.type) {
            case 'DataSource':
                AzSources.#doAddDataSource(mapRef, def as DataSourceDef, events);
                break;
            default:
                Logger.logMessage(mapRef.mapId, LogLevel.Warn, `add: unsupported source type: ${def.type}`);
                return;
        }
    }

    static #doAddDataSource(mapRef: IMapReference, def: DataSourceDef, events?: IMapEventDef[]): void {
        if (Helpers.isEmptyOrNull(def.id)) {
            Logger.logMessage(mapRef.mapId, LogLevel.Error, `addDataSource: missing Id.`, def);
            return;
        }

        let ds = mapRef.map.sources.getById(def.id);

        if (ds) {
            Logger.logMessage(mapRef.mapId, LogLevel.Warn, `addDataSource: source with ID '${def.id}' already exists.`);
            return;
        }

        const newDs = new atlas.source.DataSource(def.id, def.options);
        (newDs as any).jsInterop = def.jsInterop;

        if (events) {
            AzEvents.sources.add(mapRef, events, newDs);
        }

        mapRef.map.sources.add(newDs);

        if (def.url) {
            newDs.importDataFromUrl(def.url);
        }
    }
    // #endregion

    // #region Remove
    public static remove(mapId: string, sources: SourceDef[]): void {
        const mapRef = AzFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        const idList: string[] = [];
        sources.forEach((sourceDef) => {
            idList.push(sourceDef.id);
        });

        AzSources.#doRemoveById(mapRef, idList);
    }

    public static removeById(mapId: string, sources: string[]): void {
        if (sources.length == 0) return;

        const mapRef = AzFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        AzSources.#doRemoveById(mapRef, sources);
    }

    static #doRemoveById(mapRef: IMapReference, sources: string[]): void {
        sources.forEach((id) => {
            const source = mapRef.map.sources.getById(id);
            if (source) {
                mapRef.map.sources.remove(source);
                Logger.logMessage(mapRef.mapId, LogLevel.Debug, `remove: source with ID '${id}' was removed.`);
            }
            else {
                Logger.logMessage(mapRef.mapId, LogLevel.Warn, `remove: source with ID '${id}' was not found.`);
            }
        });
    }
    // #endregion

    // #region Clear
    public static clear(mapId: string, sources: SourceDef[]): void {
        const mapRef = AzFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        const idList: string[] = [];
        sources.forEach((sourceDef) => {
            idList.push(sourceDef.id);
        });

        AzSources.#doClearById(mapRef, idList);
    }

    public static clearById(mapId: string, sources: string[]): void {
        const mapRef = AzFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        AzSources.#doClearById(mapRef, sources);
    }

    static #doClearById(mapRef: IMapReference, sources: string[]): void {
        sources.forEach((id) => {
            const ds = mapRef.map.sources.getById(id);
            if (ds) {
                if ((ds as any).clear != undefined) {
                    (ds as any).clear();
                    Logger.logMessage(mapRef.mapId, LogLevel.Debug, `clear: source with ID '${id}' was cleared.`);
                }
                else {
                    Logger.logMessage(mapRef.mapId, LogLevel.Warn, `clear: source with ID '${id}' does not support 'clear'.`);
                }
            }
            else {
                Logger.logMessage(mapRef.mapId, LogLevel.Warn, `clear: source with ID '${id}' was not found.`);
            }
        });
    }
    // #endregion

    public static getShapes(mapId: string, id: string) {
        const mapRef = AzFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        let shapes: object[] = [];

        const ds = SourceHelper.getSource(mapRef, id);
        if (ds) {
            if (ds instanceof atlas.source.DataSource) {
                Helpers.buildShapeResults(ds.getShapes());
            }
            else {
                Logger.logMessage(mapId, LogLevel.Warn, `getShapes: source with ID '${id}' does not support 'getShapes'.`);
            }
        }

        return shapes;
    }
}

export class SourceHelper {
    static getSource(mapRef: IMapReference, id: string, logNotFound: boolean = true): atlas.source.Source | undefined {
        const source = mapRef.map.sources.getById(id);

        if (!source && logNotFound) {
            Logger.logMessage(mapRef.mapId, LogLevel.Warn, `get: source with ID '${id}' was not found.`);
        }

        return source;
    }

    static getDataSource(mapRef: IMapReference, datasourceId: string, logLevelFail: LogLevel = LogLevel.Error): atlas.source.DataSource | undefined {
        const ds = SourceHelper.getSource(mapRef, datasourceId);

        if (!ds) {
            return undefined;
        }

        if (!SourceHelper.isDataSource(ds)) {
            Logger.logMessage(mapRef.mapId, logLevelFail, `getDataSource: source with ID '${datasourceId}' is not a DataSource`);
            return undefined;
        }

        return ds;
    }

    static isDataSource(obj: any): obj is atlas.source.DataSource {
        return obj && (obj instanceof atlas.source.DataSource);
    }
}

interface SourceDef {
    id: string;
    jsInterop: IJSInteropDef;
    type: 'DataSource' | 'ElevationTile' | 'VectorTile';
}

export interface DataSourceDef extends SourceDef {
    url: string;
    options?: atlas.DataSourceOptions;
}
