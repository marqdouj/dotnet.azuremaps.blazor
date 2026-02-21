using Marqdouj.DotNet.AzureMaps.Blazor.Models.Common;
using System.Text.Json;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson
{
    /// <summary>
    /// GeoJson Extentions for Serialization.
    /// </summary>
    public static class GeoJsonSerialization
    {
        /// <summary>
        /// Generates JsonSerializerOptions for working with GeoJson objects.
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

        #region ToFeature

        /// <summary>
        /// Deserialize json string to a <see cref="Feature{G, P}"/>
        /// </summary>
        /// <typeparam name="T"><see cref="Geometry"/></typeparam>
        /// <param name="json"></param>
        /// <param name="namingPolicy"><see cref="JsonNamingPolicy"/></param>
        /// <returns></returns>
        public static Feature<T, Properties>? ToFeature<T>(this string? json, JsonNamingPolicy? namingPolicy = default) where T : Geometry
        {
            return string.IsNullOrWhiteSpace(json) ? null : JsonSerializer.Deserialize<Feature<T, Properties>>(json, GetSerializerOptions(namingPolicy));
        }

        /// <summary>
        /// Convert strongly typed Feature to generic type <see cref="Feature{G, P}"/>.
        /// </summary>
        /// <typeparam name="T"><see cref="Geometry"/></typeparam>
        /// <param name="feature">strongly typed <see cref="Feature{G, P}"/></param>
        /// <returns></returns>
        public static Feature<Geometry, Properties>? ToFeature<T>(this Feature<T, Properties>? feature) where T : Geometry
        {
            return feature is null ? null : new Feature<Geometry, Properties>(feature.Geometry, feature.Properties, feature.Id, feature.Bbox);
        }

        /// <summary>
        /// Converts <see cref="MapFeatureDef"/> to <see cref="Feature{G, P}"/>.
        /// </summary>
        /// <returns></returns>
        public static Feature<Geometry, Properties>? ToFeature(this MapFeatureDef? feature)
        {
            return feature is null ? null : new Feature<Geometry, Properties>((Geometry)feature.Geometry, feature.Properties, feature.Id, feature.Bbox);
        }

        /// <summary>
        /// Converts <see cref="MapFeatureDef"/> to a strongly typed <see cref="Feature{G, P}"/>.
        /// </summary>
        /// <typeparam name="T"><see cref="Geometry"/></typeparam>
        /// <param name="feature"><see cref="MapFeatureDef"/></param>
        /// <returns></returns>
        public static Feature<T, Properties>? ToFeature<T>(this MapFeatureDef? feature) where T: Geometry
        {
            return feature is null ? null : new Feature<T, Properties>((T)feature.Geometry, feature.Properties, feature.Id, feature.Bbox);
        }

        #endregion

        #region ToFeatureCollection

        /// <summary>
        /// Deserializes json to a strongly typed <see cref="FeatureCollection{G, P}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="namingPolicy"></param>
        /// <returns></returns>
        public static FeatureCollection<T, Properties>? ToFeatureCollection<T>(this string? json, JsonNamingPolicy? namingPolicy = default) where T : Geometry
        {
            return string.IsNullOrWhiteSpace(json) ? null : JsonSerializer.Deserialize<FeatureCollection<T, Properties>>(json, GetSerializerOptions(namingPolicy));
        }

        /// <summary>
        /// Converts a strongly typed <see cref="IEnumerable{MapFeatureDef}"/> to a strongly typed <see cref="FeatureCollection{G, P}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="featureDefs"></param>
        /// <returns></returns>
        public static FeatureCollection<T, Properties>? ToFeatureCollection<T>(this IEnumerable<MapFeatureDef<T>>? featureDefs) where T : Geometry
        {

            return featureDefs is null ? null : new FeatureCollection<T, Properties>(featureDefs.Select(e => new Feature<T, Properties>(e.Geometry, e.Properties, e.Id, e.Bbox)));
        }

        /// <summary>
        /// Converts <see cref="IEnumerable{MapFeatureDef}"/> to a <see cref="FeatureCollection{G, P}"/>.
        /// </summary>
        /// <param name="featureDefs"><see cref="MapFeatureDef"/></param>
        /// <param name="bbox"><see cref="BoundingBox"/></param>
        /// <returns></returns>
        public static FeatureCollection<Geometry, Properties>? ToFeatureCollection(this IEnumerable<MapFeatureDef>? featureDefs, BoundingBox? bbox = null)
        {
            var features = featureDefs?.Select(e => e.ToFeature());
            return features is null ? null : new FeatureCollection<Geometry, Properties>(features!, bbox);
        }

        /// <summary>
        /// Converts a strongly typed <see cref="IEnumerable{MapFeatureDef}"/> to a strongly typed <see cref="FeatureCollection{G, P}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="featureDefs"><see cref="MapFeatureDef"/></param>
        /// <param name="bbox"><see cref="BoundingBox"/></param>
        /// <returns></returns>
        public static FeatureCollection<T, Properties>? ToFeatureCollection<T>(this IEnumerable<MapFeatureDef<T>>? featureDefs, BoundingBox? bbox = null) where T : Geometry
        {
            var features = featureDefs?.Select(e => e.ToFeature<T>());
            return features is null ? null : new FeatureCollection<T, Properties>(features!, bbox);
        }

        #endregion

        #region ToMapFeatureDef

        /// <summary>
        /// Deserialize json string to a strongly typed <see cref="MapFeatureDef{T}"/>
        /// </summary>
        /// <typeparam name="T"><see cref="Geometry"/></typeparam>
        /// <param name="json"></param>
        /// <param name="namingPolicy"><see cref="JsonNamingPolicy"/></param>
        /// <returns></returns>
        public static MapFeatureDef<T>? ToMapFeatureDef<T>(this string? json, JsonNamingPolicy? namingPolicy = default) where T : Geometry
        {
            return string.IsNullOrWhiteSpace(json) ? null : JsonSerializer.Deserialize<MapFeatureDef<T>>(json, GetSerializerOptions(namingPolicy));
        }

        /// <summary>
        /// Converts <see cref="Feature{G, P}"/> to <see cref="MapFeatureDef"/>
        /// </summary>
        /// <param name="feature"><see cref="Feature{G, P}"/></param>
        /// <param name="asShape"><see cref="MapFeatureDef.AsShape"/></param>
        /// <returns></returns>
        public static MapFeatureDef? ToMapFeatureDef(this Feature<Geometry, Properties> feature, bool asShape)
        {
            if (feature is null) return null;
            if (feature.Geometry is null) return null;
            return new MapFeatureDef(feature.Geometry) { Id = $"{feature.Id}", Bbox = feature.Bbox, Properties = feature.Properties, AsShape = asShape };
        }

        /// <summary>
        /// Converts <see cref="Feature{G, P}"/> to <see cref="MapFeatureDef"/>
        /// </summary>
        /// <typeparam name="T"><see cref="Geometry"/></typeparam>
        /// <param name="feature"><see cref="Feature{G, P}"/></param>
        /// <param name="asShape"><see cref="MapFeatureDef.AsShape"/></param>
        /// <returns></returns>
        public static MapFeatureDef? ToMapFeatureDef<T>(this Feature<T, Properties> feature, bool asShape) where T : Geometry
        {
            if (feature is null) return null;
            if (feature.Geometry is null) return null;
            return new MapFeatureDef(feature.Geometry) { Id = $"{feature.Id}", Bbox = feature.Bbox, Properties = feature.Properties, AsShape = asShape };
        }

        #endregion

        #region ToMapFeatureDefs

        /// <summary>
        /// Converts a strongly typed <see cref="FeatureCollection{G, P}"/> to <see cref="List{MapFeatureDef}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"><see cref="FeatureCollection{G, P}"/></param>
        /// <param name="asShape"><see cref="MapFeatureDef.AsShape"/></param>
        /// <returns></returns>
        public static List<MapFeatureDef>? ToMapFeatureDefs<T>(this FeatureCollection<T, Properties>? collection, bool asShape) where T: Geometry
        {
            if (collection is null) return null;
            if (collection.Features is null) return null;

            return [.. collection.Features.Where(e => e.Geometry is not null).Select(e => e.ToMapFeatureDef<T>(asShape)!)];
        }

        /// <summary>
        /// Deserialize json string to a strongly typed list of <see cref="MapFeatureDef{T}"/>.
        /// </summary>
        /// <typeparam name="T"><see cref="Geometry"/></typeparam>
        /// <param name="json"></param>
        /// <param name="namingPolicy"><see cref="JsonNamingPolicy"/></param>
        /// <returns></returns>
        public static List<MapFeatureDef<T>>? ToMapFeatureDefs<T>(this string? json, JsonNamingPolicy? namingPolicy = default) where T: Geometry
        {
            return string.IsNullOrWhiteSpace(json) ? null : JsonSerializer.Deserialize<List<MapFeatureDef<T>>>(json, GetSerializerOptions(namingPolicy));
        }

        #endregion

        #region ToJson Feature

        /// <summary>
        /// Serializes a <see cref="Feature{G, P}"/>
        /// </summary>
        /// <param name="feature"><see cref="Feature{G, P}"/></param>
        /// <param name="namingPolicy"><see cref="JsonNamingPolicy"/></param>
        /// <param name="writeIndented"><see cref="JsonSerializerOptions.WriteIndented"/>/param>
        /// <returns></returns>
        public static string? ToJson(this Feature<Geometry, Properties>? feature, JsonNamingPolicy? namingPolicy = default, bool writeIndented = true)
        {
            if (feature is null) return null;
            return JsonSerializer.Serialize(feature, GetSerializerOptions(namingPolicy, writeIndented));
        }

        /// <summary>
        /// Serializes a strongly typed <see cref="Feature{G, P}"/>
        /// </summary>
        /// <typeparam name="T"><see cref="Geometry"/></typeparam>
        /// <param name="feature"><see cref="Feature{G, P}"/></param>
        /// <param name="namingPolicy"><see cref="JsonNamingPolicy"/></param>
        /// <param name="writeIndented"><see cref="JsonSerializerOptions.WriteIndented"/>/param>
        /// <returns></returns>
        public static string? ToJson<T>(this Feature<T, Properties>? feature, JsonNamingPolicy? namingPolicy = default, bool writeIndented = true) where T : Geometry
        {
            if (feature is null) return null;
            return JsonSerializer.Serialize(feature, GetSerializerOptions(namingPolicy, writeIndented));
        }

        #endregion

        #region ToJson Features

        /// <summary>
        /// Serializes a <see cref="FeatureCollection{G, P}"/>.
        /// </summary>
        /// <param name="collection"><see cref="FeatureCollection{G, P}"/></param>
        /// <param name="namingPolicy"><see cref="JsonNamingPolicy"/></param>
        /// <param name="writeIndented"><see cref="JsonSerializerOptions.WriteIndented"/>/param>
        /// <returns></returns>
        public static string? ToJson(this FeatureCollection<Geometry, Properties>? collection, JsonNamingPolicy? namingPolicy = default, bool writeIndented = true)
        {
            return collection is null ? null : JsonSerializer.Serialize(collection, GetSerializerOptions(namingPolicy, writeIndented));
        }

        /// <summary>
        /// Serializes a strongly typed <see cref="FeatureCollection{G, P}"/>.
        /// </summary>
        /// <typeparam name="T"><see cref="Geometry"/></typeparam>
        /// <param name="collection"><see cref="FeatureCollection{G, P}"/></param>
        /// <param name="namingPolicy"><see cref="JsonNamingPolicy"/></param>
        /// <param name="writeIndented"><see cref="JsonSerializerOptions.WriteIndented"/>/param>
        /// <returns></returns>
        public static string? ToJson<T>(this FeatureCollection<T, Properties>? collection, JsonNamingPolicy? namingPolicy = default, bool writeIndented = true) where T: Geometry
        {
            return collection is null ? null : JsonSerializer.Serialize(collection, GetSerializerOptions(namingPolicy, writeIndented));
        }

        #endregion

        #region ToJson MapFeatureDef

        /// <summary>
        /// Serializes a <see cref="MapFeatureDef"/>.
        /// </summary>
        /// <param name="mapFeatureDef"><see cref="MapFeatureDef"/></param>
        /// <param name="options"><see cref="JsonSerializerOptions"/></param>
        /// <returns></returns>
        public static string? ToJson(this MapFeatureDef? mapFeatureDef, JsonSerializerOptions? options = null)
        {
            return mapFeatureDef is null ? null : JsonSerializer.Serialize(mapFeatureDef, options ?? GetSerializerOptions());
        }

        /// <summary>
        /// Serializes a <see cref="MapFeatureDef"/>.
        /// </summary>
        /// <param name="mapFeatureDef"><see cref="MapFeatureDef"/></param>
        /// <param name="namingPolicy"><see cref="JsonNamingPolicy"/></param>
        /// <param name="writeIndented"><see cref="JsonSerializerOptions.WriteIndented"/></param>
        /// <returns></returns>
        public static string? ToJson(this MapFeatureDef? mapFeatureDef, JsonNamingPolicy? namingPolicy = default, bool writeIndented = true)
        {
            return mapFeatureDef is null ? null : JsonSerializer.Serialize(mapFeatureDef, GetSerializerOptions(namingPolicy, writeIndented));
        }

        #endregion

        #region ToJson MapFeatureDefs

        /// <summary>
        /// Serializes an <see cref="IEnumerable{MapFeatureDef}"/>.
        /// </summary>
        /// <param name="features"><see cref="MapFeatureDef"/></param>
        /// <param name="namingPolicy"><see cref="JsonNamingPolicy"/></param>
        /// <param name="writeIndented"><see cref="JsonSerializerOptions.WriteIndented"/></param>
        /// <returns></returns>
        public static string? ToJson(this IEnumerable<MapFeatureDef>? features, JsonNamingPolicy? namingPolicy = default, bool writeIndented = true) 
        {
            return features is null ? null : JsonSerializer.Serialize(features, GetSerializerOptions(namingPolicy, writeIndented));
        }

        /// <summary>
        /// Serializes an <see cref="IEnumerable{MapFeatureDef}"/>.
        /// </summary>
        /// <param name="features"><see cref="IEnumerable{MapFeatureDef}"/></param>
        /// <param name="options"><see cref="JsonSerializerOptions"/></param>
        /// <returns></returns>
        public static string? ToJson(this IEnumerable<MapFeatureDef>? features, JsonSerializerOptions? options)
        {
            return features is null ? null : JsonSerializer.Serialize(features, options ?? GetSerializerOptions());
        }

        #endregion
    }
}
