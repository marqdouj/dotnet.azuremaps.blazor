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
        [JsonPropertyOrder(-1)]
        [JsonInclude]
        public virtual GeoJsonType Type { get; internal set; } = type;

        //[JsonInclude]
        //[JsonPropertyName("Type")]
        //internal string? TypeJs { get => Type.ToString(); set => Type = value.JsonToEnum<GeoJsonType>(); }

        /// <summary>
        /// <see cref="BoundingBox"/>
        /// </summary>
        public BoundingBox? Bbox { get; set; }
    }
}
