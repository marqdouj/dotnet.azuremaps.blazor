using Marqdouj.DotNet.AzureMaps.Blazor.Models.Common;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson
{
    /// <summary>
    /// <see cref="MapFeatureDef"/>. Required for Deserialization.
    /// </summary>
    /// <typeparam name="T"><see cref="GeoJson.Geometry"/></typeparam>
    /// <param name="geometry"><see cref="GeoJson.Geometry"/></param>
    public class MapFeatureDef<T>(T geometry) : MapFeatureDef(geometry) where T : Geometry
    {
        /// <summary>
        /// <see cref="MapFeatureDef.Geometry"/>
        /// </summary>
        public override T Geometry => (T)base.Geometry;
    }

    /// <summary>
    /// Defines a geometry feature to be added to a data source.
    /// </summary>
    /// <param name="geometry"></param>
    public class MapFeatureDef (Geometry geometry) : JsInteropBase
    {
        /// <summary>
        /// Identifier for the feature.
        /// </summary>
        public override string Id { get => string.IsNullOrWhiteSpace(field) ? JsInterop.Id : field; set; } = string.Empty;

        /// <summary>
        /// <see cref="GeoJson.Geometry"/>
        /// </summary>
        public virtual object Geometry { get; } = geometry;

        /// <summary>
        /// <see cref="GeoJson.GeometryType"/>
        /// </summary>
        [JsonIgnore]
        public GeometryType GeometryType => ((Geometry)Geometry).Type;

        /// <summary>
        /// Gets the coordinates for the underlying <see cref="GeoJson.GeometryType"/> (Point, LineString, ...).
        /// </summary>
        [JsonIgnore]
        public object Coordinates => GetCoordinates();

        /// <summary>
        /// <see cref="BoundingBox"/>
        /// </summary>
        public BoundingBox? Bbox { get; set; }

        /// <summary>
        /// <see cref="Common.Properties"/>
        /// </summary>
        public Properties? Properties { get; set; }

        /// <summary>
        /// When true, the feature will be added as a shape to the data source. 
        /// This is useful for features that require support for the additional functionality
        /// that a shape provides, such as editing and event handling.
        /// </summary>
        public bool AsShape { get; set; }

        private object GetCoordinates()
        {
            var geometry = (Geometry)Geometry;

            return GeometryType switch
            {
                GeometryType.Point => ((Point)geometry).Coordinates,
                GeometryType.MultiPoint => ((MultiPoint)geometry).Coordinates,
                GeometryType.LineString => ((LineString)geometry).Coordinates,
                GeometryType.MultiLineString => ((MultiLineString)geometry).Coordinates,
                GeometryType.Polygon => ((Polygon)geometry).Coordinates,
                GeometryType.MultiPolygon => ((MultiPolygon)geometry).Coordinates,
                _ => throw new ArgumentOutOfRangeException(nameof(GeometryType)),
            };
        }
    }
}
