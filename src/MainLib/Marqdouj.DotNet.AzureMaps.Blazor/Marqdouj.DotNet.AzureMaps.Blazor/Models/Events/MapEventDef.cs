using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Events
{
    public enum MapEventTarget
    {
        Map,
        DataSource,
        HtmlMarker,
        Layer,
        Popup,
        Shape,
        StyleControl,
    }

    /// <summary>
    /// Defines the configuration for a map event, including its type, target, and event handling options.
    /// </summary>
    /// <remarks>Use this class to specify the details of a map event to be registered or handled, such as the
    /// event type, the target element, and options like whether to prevent the default browser behavior or handle the
    /// event only once. This type is typically used when subscribing to or describing events in a mapping
    /// component.</remarks>
    public class MapEventDef : MapEventBase, ICloneable
    {
        [JsonConstructor]
        public MapEventDef() { }

        public MapEventDef(MapEventType type, MapEventTarget target)
        {
            Type = type;
            Target = target;
        }

        /// <summary>
        /// If true and the js event supports it, preventDefault will be applied to the event.
        /// i.e. Mouse, Touch, and Wheel events.
        /// </summary>
        public bool PreventDefault { get; set; }

        /// <summary>
        /// Required for targets that belong to a source, i.e. Shape requires DataSourceId.
        /// </summary>
        public string? TargetSourceId { get; set; }

        /// <summary>
        /// If <see langword="true"/> adds the event once (for events that support 'once'); otherwise continuous. Default is <see langword="false"/>.
        /// </summary>
        public bool Once { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
