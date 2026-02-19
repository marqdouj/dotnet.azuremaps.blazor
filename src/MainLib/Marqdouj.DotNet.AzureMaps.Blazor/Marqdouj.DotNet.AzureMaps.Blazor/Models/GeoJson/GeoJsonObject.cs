using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson
{
    /// <summary>
    /// The base GeoJSON object containing the "type" discriminator and optional bounding box.
    /// All other geojson classes inherit from this.
    /// </summary>
    public abstract class GeoJsonObject(GeoJsonType type)
    {
        /// <summary>
        /// <see cref="GeoJsonType"/>
        /// </summary>
        [JsonIgnore]
        public virtual GeoJsonType Type { get; protected set; } = type;

        [JsonInclude]
        [JsonPropertyName("type")]
        internal string? TypeJs { get => Type.EnumToJson(); set => Type = value.JsonToEnum<GeoJsonType>(); }

        /// <summary>
        /// <see cref="BoundingBox"/>
        /// </summary>
        public BoundingBox? BBox { get; set; }
    }
}
