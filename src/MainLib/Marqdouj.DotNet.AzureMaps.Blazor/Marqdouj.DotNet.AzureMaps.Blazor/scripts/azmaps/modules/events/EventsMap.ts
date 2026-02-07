import { Logger, LogLevel } from "../../common/Logger";
import { IMapEventDef } from "../../typings";

export class EventsMap {
    #eventsMap: Map<string, object> = new Map<string, object>();
    mapId: string;

    constructor(mapId: string) { this.mapId = mapId; }

    addCallback(eventDef: IMapEventDef, callback: any) {
        const eventId = this.#getCallbackId(eventDef);
        if (!this.#eventsMap.has(eventId)) {
            this.#eventsMap.set(eventId, callback);
            Logger.logMessage(this.mapId, LogLevel.Trace, "EventsMap-addCallback:", eventId);
        }
    }

    getCallback(eventDef: IMapEventDef, removing: boolean) {
        const eventId = this.#getCallbackId(eventDef);
        if (this.#eventsMap.has(eventId)) {
            const callback = this.#eventsMap.get(eventId);
            if (removing) {
                const wasRemoved = this.#eventsMap.delete(eventId);
                Logger.logMessage(this.mapId, LogLevel.Trace, "EventsMap-removeCallback:", eventId, wasRemoved);
            }
            return callback;
        }
    }

    #getCallbackId(eventDef: IMapEventDef) {
        return `${eventDef.target}.${eventDef.type}.${eventDef.targetId}`;
    }

    clear() {
        this.#eventsMap.clear();
    }
}