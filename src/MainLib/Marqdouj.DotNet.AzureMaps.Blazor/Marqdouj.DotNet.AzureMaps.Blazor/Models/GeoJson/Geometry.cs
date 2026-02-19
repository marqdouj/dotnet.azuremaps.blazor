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
        [JsonIgnore]
        public new GeometryType Type { get => (GeometryType)base.Type; protected set => base.Type = (GeoJsonType)value; }
    }
}