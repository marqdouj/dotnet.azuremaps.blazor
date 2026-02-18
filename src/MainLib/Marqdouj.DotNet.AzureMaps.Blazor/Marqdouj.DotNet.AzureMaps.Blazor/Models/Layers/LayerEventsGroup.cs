using Marqdouj.DotNet.AzureMaps.Blazor.Models.Events;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers
{
    /// <summary>
    /// Represents a group of events associated with a specific map layer.
    /// </summary>
    public class LayerEventsGroup
    {
        /// <summary>
        /// Initializes a new instance of the LayerEventsGroup class with the specified map layer and associated
        /// layer-level events.
        /// </summary>
        /// <remarks>Each event in the provided collection is cloned and its target and target identifier
        /// are set to match the specified layer. Events not targeting the layer are ignored.</remarks>
        /// <param name="layer">The map layer definition to associate with this group. Cannot be null.</param>
        /// <param name="events">A collection of map event definitions to associate with the layer. Only events targeting the layer are
        /// included. Can be null.</param>
        public LayerEventsGroup(MapLayerDef layer, IEnumerable<MapEventDef>? events = null)
        {
            Layer = layer;
            Events = events?
                .Where(e => e.Target == MapEventTarget.Layer)
                .Select(e => (MapEventDef)e.Clone())
                .ToList() ?? [];

            foreach (var item in Events)
            {
                item.Target = MapEventTarget.Layer;
                item.TargetId = layer.Id;
            }
        }

        /// <summary>
        /// Initializes a new instance of the LayerEventsGroup class for the specified map layer and event types.
        /// </summary>
        /// <param name="layer">The map layer definition to associate with this group. Cannot be null.</param>
        /// <param name="events">A collection of event types to include in the group, or null to specify no events.</param>
        public LayerEventsGroup(MapLayerDef layer, IEnumerable<MapEventType>? events)
            : this(layer, events?.ToMapEventDefs(MapEventTarget.Layer)) { }

        /// <summary>
        /// Initializes a new instance of the LayerEventsGroup class for the specified map layer and associated event
        /// types.
        /// </summary>
        /// <param name="layer">The map layer definition to associate with this group. Cannot be null.</param>
        /// <param name="events">A collection of event type layers to associate with the map layer, or null to indicate no events.</param>
        public LayerEventsGroup(MapLayerDef layer, IEnumerable<MapEventTypeLayer>? events)
            : this(layer, events?.Cast<MapEventType>().ToMapEventDefs(MapEventTarget.Layer)) { }

        public MapLayerDef Layer { get; }
        public List<MapEventDef> Events { get; }
        public MapLayerType LayerType => Layer.LayerType;
    }
}
