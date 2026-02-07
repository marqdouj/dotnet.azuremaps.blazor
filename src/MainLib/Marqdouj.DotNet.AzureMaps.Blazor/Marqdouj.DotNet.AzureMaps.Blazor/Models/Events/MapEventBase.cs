using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Events
{
    public abstract class MapEventBase
    {
        /// <summary>
        /// <see cref="MapEventType"/>
        /// </summary>
        [JsonIgnore]
        public MapEventType? Type { get; internal set; }

        [JsonInclude]
        [JsonPropertyName("type")]
        internal string? TypeJs { get => Type.EnumToJsonN(); set => Type = value.JsonToEnumN<MapEventType>(); }

        /// <summary>
        /// Type of map object associated with the event.
        /// </summary>
        [JsonIgnore]
        public MapEventTarget? Target { get; internal set; }

        [JsonInclude]
        [JsonPropertyName("target")]
        internal string? TargetJs { get => Target.EnumToJsonN(); set => Target = value.JsonToEnumN<MapEventTarget>(); }

        /// <summary>
        /// Required for any Target other than 'Map'.
        /// For the StyleControl use the 'InteropId'
        /// </summary>
        public string? TargetId { get; set; }
    }
}
