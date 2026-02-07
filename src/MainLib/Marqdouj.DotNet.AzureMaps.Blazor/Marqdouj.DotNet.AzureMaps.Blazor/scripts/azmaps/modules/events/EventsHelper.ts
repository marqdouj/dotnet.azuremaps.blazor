import * as atlas from "azure-maps-control";
import { Helpers } from "../../common/Helpers";
import { IMapEventDef, MapEventArgs } from "../../typings";

export class EventsHelper {
    static buildKeyboardEventPayload(callback: KeyboardEvent) {
        const payload = {
            key: callback.key,
            code: callback.code,
            location: callback.location,
            repeat: callback.repeat,
            altKey: callback.altKey,
            ctrlKey: callback.ctrlKey,
            shiftKey: callback.shiftKey,
            metaKey: callback.metaKey
        };

        return { keyboard: payload };
    }

    static buildMapEventArgs(mapId: string, event: IMapEventDef, payload?: any, source?: any): MapEventArgs {
        const args: MapEventArgs =
        {
            mapId: mapId,
            type: event.type,
            target: event.target,
            targetId: event?.targetId,
            payload: { jsInterop: Helpers.getJsInteropDef(source) ,...payload }
        };

        return args;
    }

    static buildLayerEventPayload(layer: atlas.layer.Layer) {
        return { layer: { id: layer.getId() } };
    }

    static buildShapeResults(shapes: Array<atlas.data.Feature<atlas.data.Geometry, any> | atlas.Shape>): object[] {
        const results: object[] = [];

        shapes.filter(feature => Helpers.isFeature(feature)).forEach(feature => {
            results.push(Helpers.getFeatureResult(feature));
        });
        shapes.filter(shape => Helpers.isShape(shape)).forEach(shape => {
            results.push(Helpers.getShapeResult(shape));
        });

        return results;
    }

    static buildMouseEventPayload(mouseEvent: atlas.MapMouseEvent) {
        const mouse = {
            layerId: mouseEvent.layerId,
            pixel: mouseEvent.pixel,
            position: mouseEvent.position,
            shapes: this.buildShapeResults(mouseEvent.shapes)
        };

        return { mouse: mouse };
    }

    static buildTouchEventPayload(touchEvent: atlas.MapTouchEvent) {
        const payload = {
            pixel: touchEvent.pixel,
            pixels: touchEvent.pixels,
            position: touchEvent.position,
            positions: touchEvent.positions,
            layerId: touchEvent.layerId,
            shapes: this.buildShapeResults(touchEvent.shapes)
        };

        return { touch: payload };
    }

    static buildWheelEventPayload(wheelEvent: atlas.MapMouseWheelEvent) {
        const payload = {
            type: wheelEvent.type,
        };

        return { wheel: payload };
    }
}
