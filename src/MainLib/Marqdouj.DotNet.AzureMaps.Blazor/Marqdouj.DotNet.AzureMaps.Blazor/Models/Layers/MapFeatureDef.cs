using Marqdouj.DotNet.AzureMaps.Blazor.Models.Common;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers
{
    /// <summary>
    /// <see cref="MapFeatureDef"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="geometry"><see cref="MapFeatureDef.Geometry"/></param>
    public class MapFeatureDef<T>(T geometry) : MapFeatureDef(geometry) where T : Geometry
    {
        /// <summary>
        /// <see cref="MapFeatureDef.Geometry"/>
        /// </summary>
        public override T Geometry => (T)base.Geometry;
    }

    /// <summary>
    /// Defines a feature to be added to a data source.
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

    /// <summary>
    /// MapFeatureDef Extensions
    /// </summary>
    public static class MapFeatureDefExtensions
    {
        /// <summary>
        /// Generates JsonSerializerOptions for working with <see cref="MapFeatureDef"/>
        /// </summary>
        /// <param name="namingPolicy"><see cref="JsonNamingPolicy"/></param>
        /// <param name="writeIndented"><see cref="JsonSerializerOptions.WriteIndented"/>/param>
        /// <returns></returns>
        private static JsonSerializerOptions GetSerializerOptions(JsonNamingPolicy? namingPolicy = default, bool writeIndented = true)
        {
            return new JsonSerializerOptions(JsonSerializerDefaults.Web) 
            {
                PropertyNamingPolicy = namingPolicy,
                WriteIndented = writeIndented
            };
        }

        /// <summary>
        /// Serializes a <![CDATA[FeatureCollection<Geometry, Properties>]]>.
        /// </summary>
        /// <param name="collection"><see cref="FeatureCollection{G, P}"/></param>
        /// <param name="namingPolicy"><see cref="JsonNamingPolicy"/></param>
        /// <param name="writeIndented"><see cref="JsonSerializerOptions.WriteIndented"/>/param>
        /// <returns></returns>
        public static string Serialize(this FeatureCollection<Geometry, Properties> collection, JsonNamingPolicy? namingPolicy = default, bool writeIndented = true)
        {
            return JsonSerializer.Serialize(collection, GetSerializerOptions(namingPolicy, writeIndented));
        }

        /// <summary>
        /// Serializes an <![CDATA[IList<MapFeatureDef<T>>]]>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="features"><see cref="MapFeatureDef{T}"/></param>
        /// <param name="namingPolicy"><see cref="JsonNamingPolicy"/></param>
        /// <param name="writeIndented"><see cref="JsonSerializerOptions.WriteIndented"/></param>
        /// <returns></returns>
        public static string Serialize<T>(this IEnumerable<MapFeatureDef<T>> features, JsonNamingPolicy? namingPolicy = default, bool writeIndented = true) where T : Geometry
        {
            return JsonSerializer.Serialize(features, GetSerializerOptions(namingPolicy, writeIndented));
        }

        /// <summary>
        /// Serialize a <see cref="Feature{G, P}"/>
        /// </summary>
        /// <param name="feature"><see cref="Feature{G, P}"/></param>
        /// <param name="namingPolicy"><see cref="JsonNamingPolicy"/></param>
        /// <param name="writeIndented"><see cref="JsonSerializerOptions.WriteIndented"/>/param>
        /// <returns></returns>
        public static string Serialize(this Feature<Geometry, Properties> feature, JsonNamingPolicy? namingPolicy = default, bool writeIndented = true)
        {
            return JsonSerializer.Serialize(feature, GetSerializerOptions(namingPolicy, writeIndented));
        }

        /// <summary>
        /// Deserialize json string to a <see cref="MapFeatureDef{T}"/>
        /// </summary>
        /// <typeparam name="T"><see cref="Geometry"/></typeparam>
        /// <param name="json"></param>
        /// <param name="namingPolicy"><see cref="JsonNamingPolicy"/></param>
        /// <returns></returns>
        public static MapFeatureDef<T>? ToMapFeatureDef<T>(this string json, JsonNamingPolicy? namingPolicy = default) where T : Geometry
        {
            return JsonSerializer.Deserialize<MapFeatureDef<T>>(json, GetSerializerOptions(namingPolicy));
        }

        /// <summary>
        /// Deserialize json string to a list of <see cref="MapFeatureDef{T}"/>.
        /// </summary>
        /// <typeparam name="T"><see cref="Geometry"/></typeparam>
        /// <param name="json"></param>
        /// <param name="namingPolicy"><see cref="JsonNamingPolicy"/></param>
        /// <returns></returns>
        public static List<MapFeatureDef<T>>? ToMapFeatureDefs<T>(this string json, JsonNamingPolicy? namingPolicy = default) where T : Geometry
        {
            return JsonSerializer.Deserialize<List<MapFeatureDef<T>>>(json, GetSerializerOptions(namingPolicy));
        }

        /// <summary>
        /// Serializes the MapFeatureDef.
        /// </summary>
        /// <param name="mapFeatureDef"><see cref="MapFeatureDef"/></param>
        /// <param name="options"><see cref="JsonSerializerOptions"/></param>
        /// <returns></returns>
        public static string Serialize(this MapFeatureDef mapFeatureDef, JsonSerializerOptions? options = null)
        {
            return JsonSerializer.Serialize(mapFeatureDef, options ?? GetSerializerOptions());
        }

        /// <summary>
        /// Serializes the MapFeatureDef.
        /// </summary>
        /// <param name="mapFeatureDef"><see cref="MapFeatureDef"/></param>
        /// <param name="namingPolicy"><see cref="JsonNamingPolicy"/></param>
        /// <param name="writeIndented"><see cref="JsonSerializerOptions.WriteIndented"/></param>
        /// <returns></returns>
        public static string Serialize(this MapFeatureDef mapFeatureDef, JsonNamingPolicy? namingPolicy = default, bool writeIndented = true)
        {
            return JsonSerializer.Serialize(mapFeatureDef, GetSerializerOptions(namingPolicy, writeIndented));
        }

        /// <summary>
        /// Converts <see cref="MapFeatureDef"/> to <![CDATA[ Feature<Geometry, Properties>]]>.
        /// </summary>
        /// <returns></returns>
        public static Feature<Geometry, Properties> ToFeature(this MapFeatureDef feature)
        {
            return new Feature<Geometry, Properties>((Geometry)feature.Geometry, feature.Properties, feature.Id, feature.Bbox);
        }

        /// <summary>
        /// Converts <![CDATA[ Feature<Geometry, Properties>]]> to <see cref="MapFeatureDef"/>
        /// </summary>
        /// <param name="feature"><see cref="Feature{G, P}"/></param>
        /// <param name="asShape"><see cref="MapFeatureDef.AsShape"/></param>
        /// <returns></returns>
        public static MapFeatureDef FromFeature(this Feature<Geometry, Properties> feature, bool asShape)
        {
            ArgumentNullException.ThrowIfNull(feature.Geometry);
            return new MapFeatureDef(feature.Geometry) { Bbox = feature.Bbox, Properties = feature.Properties, AsShape = asShape};
        }

        /// <summary>
        /// Converts IEnumerable{MapFeatureDef} to a FeatureCollection.
        /// </summary>
        /// <param name="features"><see cref="Feature{G, P}"/></param>
        /// <param name="bbox"><see cref="BoundingBox"/></param>
        /// <returns></returns>
        public static FeatureCollection<Geometry, Properties> ToFeatureCollection(this IEnumerable<MapFeatureDef> features, BoundingBox? bbox = null)
        {
            return new FeatureCollection<Geometry, Properties>(features.Select(e => e.ToFeature()), bbox);
        }

        /// <summary>
        /// Converts <![CDATA[FeatureCollection<Geometry, Properties>]]> to <![CDATA[List<MapFeatureDef>]]>
        /// </summary>
        /// <param name="collection"><see cref="FeatureCollection{G, P}"/></param>
        /// <param name="asShape"><see cref="MapFeatureDef.AsShape"/></param>
        /// <returns></returns>
        public static List<MapFeatureDef> FromFeatureCollection(this FeatureCollection<Geometry, Properties> collection, bool asShape)
        {
            return [.. collection.Features.Select(e => e.FromFeature(asShape))];
        }
    }
}
