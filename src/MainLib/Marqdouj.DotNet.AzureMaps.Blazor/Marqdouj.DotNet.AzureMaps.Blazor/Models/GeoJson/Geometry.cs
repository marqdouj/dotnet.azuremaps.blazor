using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson
{
    /// <summary>
    /// Base class for all Geometry types (Point, MultiPoint, LineString, ...).
    /// </summary>
    public abstract class Geometry(GeometryType type) : GeoJsonObject((GeoJsonType)type)
    {
        /// <summary>
        /// <see cref="Geometry"/>
        /// </summary>
        [JsonPropertyOrder(-1)]
        [JsonInclude]
        public new GeometryType Type { get => (GeometryType)base.Type; internal set => base.Type = (GeoJsonType)value; }
    }
}