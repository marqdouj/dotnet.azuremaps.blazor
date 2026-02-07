namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Events
{
    public static class MapEventExtensions
    {
        /// <summary>
        /// Converts a collection of map event types to a list of map event definitions with the specified target.
        /// </summary>
        /// <param name="events">The collection of map event types to convert. If null, an empty list is returned.</param>
        /// <param name="target">The target to assign to each map event definition. Defaults to MapEventTarget.Map.</param>
        /// <returns>A list of MapEventDef objects representing the specified event types and target. Returns an empty list if
        /// events is null or empty.</returns>
        public static List<MapEventDef> ToMapEventDefs(this IEnumerable<MapEventType>? events, MapEventTarget target = MapEventTarget.Map)
        {
            if (events == null) return [];

            return [.. events.Select(e => new MapEventDef
            {
                Type = e,
                Target = target,
            })];
        }

        /// <summary>
        /// Returns a MapEventDef list of all MapEventTypes that apply to the target.
        /// </summary>
        /// <param name="target"><see cref="MapEventTarget"/></param>
        /// <param name="preventDefault"><see cref="MapEventDef.PreventDefault"/></param>
        /// <returns></returns>
        public static IEnumerable<MapEventDef> GetMapEventDefs(this MapEventTarget target, bool preventDefault = true)
        {
            return target switch
            {
                MapEventTarget.Map => Enum.GetValues<MapEventTypeMap>().Select(e => new MapEventDef((MapEventType)e, MapEventTarget.Map) { PreventDefault = preventDefault }).OrderBy(e => e.TypeJs),
                MapEventTarget.DataSource => Enum.GetValues<MapEventTypeDataSource>().Select(e => new MapEventDef((MapEventType)e, MapEventTarget.DataSource) { PreventDefault = preventDefault }).OrderBy(e => e.TypeJs),
                MapEventTarget.HtmlMarker => Enum.GetValues<MapEventTypeHtmlMarker>().Select(e => new MapEventDef((MapEventType)e, MapEventTarget.HtmlMarker) { PreventDefault = preventDefault }).OrderBy(e => e.TypeJs),
                MapEventTarget.Layer => Enum.GetValues<MapEventTypeLayer>().Select(e => new MapEventDef((MapEventType)e, MapEventTarget.Layer) { PreventDefault = preventDefault }).OrderBy(e => e.TypeJs),
                MapEventTarget.Popup => Enum.GetValues<MapEventTypePopup>().Select(e => new MapEventDef((MapEventType)e, MapEventTarget.Popup) { PreventDefault = preventDefault }).OrderBy(e => e.TypeJs),
                MapEventTarget.Shape => Enum.GetValues<MapEventTypeShape>().Select(e => new MapEventDef((MapEventType)e, MapEventTarget.Shape) { PreventDefault = preventDefault }).OrderBy(e => e.TypeJs),
                MapEventTarget.StyleControl => Enum.GetValues<MapEventTypeStyleControl>().Select(e => new MapEventDef((MapEventType)e, MapEventTarget.StyleControl) { PreventDefault = preventDefault }).OrderBy(e => e.TypeJs),
                _ => throw new ArgumentException($"Invalid MapEventTarget: '{target}'"),
            };
        }

        /// <summary>
        /// Returns a MapEventType list of all MapEventTypes that apply to the target.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static IEnumerable<MapEventType> GetMapEventTypes(this MapEventTarget target)
        {
            return target switch
            {
                MapEventTarget.Map => Enum.GetValues<MapEventTypeMap>().Cast<MapEventType>().OrderBy(e => e.ToString()),
                MapEventTarget.DataSource => Enum.GetValues<MapEventTypeDataSource>().Cast<MapEventType>().OrderBy(e => e.ToString()),
                MapEventTarget.HtmlMarker => Enum.GetValues<MapEventTypeHtmlMarker>().Cast<MapEventType>().OrderBy(e => e.ToString()),
                MapEventTarget.Layer => Enum.GetValues<MapEventTypeLayer>().Cast<MapEventType>().OrderBy(e => e.ToString()),
                MapEventTarget.Popup => Enum.GetValues<MapEventTypePopup>().Cast<MapEventType>().OrderBy(e => e.ToString()),
                MapEventTarget.Shape => Enum.GetValues<MapEventTypeShape>().Cast<MapEventType>().OrderBy(e => e.ToString()),
                MapEventTarget.StyleControl => Enum.GetValues<MapEventTypeStyleControl>().Cast<MapEventType>().OrderBy(e => e.ToString()),
                _ => throw new ArgumentException($"Invalid MapEventTarget: '{target}'"),
            };
        }
    }
}
