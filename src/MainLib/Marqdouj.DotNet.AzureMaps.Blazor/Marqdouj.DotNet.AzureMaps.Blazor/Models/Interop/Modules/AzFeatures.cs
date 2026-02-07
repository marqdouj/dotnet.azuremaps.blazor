using Marqdouj.DotNet.AzureMaps.Blazor.Models.Common;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop.Modules
{
    public interface IAzMapFeatures
    {
        Task Add(IEnumerable<MapFeatureDef> features, string datasourceId, bool replace = false);
        Task Add(MapFeatureDef feature, string datasourceId, bool replace = false);
        Task AddProperty(MapFeatureDef feature, string name, object? value, string datasourceId);
        Task<object> GetCoordinates(MapFeatureDef feature, string datasourceId);
        Task<Properties> GetProperties(MapFeatureDef feature, string datasourceId);
        Task SetCoordinates(MapFeatureDef feature, string datasourceId);
        Task SetProperties(MapFeatureDef feature, string datasourceId, bool replace = false);
        Task Update(IEnumerable<MapFeatureDef> features, string datasourceId);
        Task Update(MapFeatureDef feature, string datasourceId);
    }

    internal class AzFeatures(IAzMapModuleReferences dotNetRef, string mapId) : IAzMapFeatures
    {
        private readonly IAzMapModuleReferences dotNetRef = dotNetRef;
        private readonly string mapId = mapId;
        private IJSRuntime JsRuntime => dotNetRef.JsRuntime;

        public async Task Add(MapFeatureDef feature, string datasourceId, bool replace = false)
        {
            await Add([feature], datasourceId, replace);
        }

        public async Task Add(IEnumerable<MapFeatureDef> features, string datasourceId, bool replace = false)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, features, datasourceId, replace);
        }

        public async Task Update(MapFeatureDef feature, string datasourceId)
        {
            await Update([feature], datasourceId);
        }

        public async Task Update(IEnumerable<MapFeatureDef> features, string datasourceId)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, features, datasourceId);
        }

        public async Task AddProperty(MapFeatureDef feature, string name, object? value, string datasourceId)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, feature.Id, name, value, datasourceId);
        }

        public async Task<Properties> GetProperties(MapFeatureDef feature, string datasourceId)
        {
            return await JsRuntime.InvokeAsync<Properties>(GetJsInteropMethod(), mapId, feature.Id, datasourceId);
        }

        public async Task SetProperties(MapFeatureDef feature, string datasourceId, bool replace = false)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, feature.Id, feature.Properties, datasourceId, replace);
        }

        public async Task<object> GetCoordinates(MapFeatureDef feature, string datasourceId)
        {
            return feature.GeometryType switch
            {
                GeometryType.Point => await JsRuntime.InvokeAsync<Position>(GetJsInteropMethod(), mapId, feature.Id, datasourceId),
                GeometryType.MultiPoint => await JsRuntime.InvokeAsync<List<Position>>(GetJsInteropMethod(), mapId, feature.Id, datasourceId),
                GeometryType.LineString => await JsRuntime.InvokeAsync<List<Position>>(GetJsInteropMethod(), mapId, feature.Id, datasourceId),
                GeometryType.MultiLineString => await JsRuntime.InvokeAsync<List<List<Position>>>(GetJsInteropMethod(), mapId, feature.Id, datasourceId),
                GeometryType.Polygon => await JsRuntime.InvokeAsync<List<List<Position>>>(GetJsInteropMethod(), mapId, feature.Id, datasourceId),
                GeometryType.MultiPolygon => await JsRuntime.InvokeAsync<List<List<List<Position>>>>(GetJsInteropMethod(), mapId, feature.Id, datasourceId),
                _ => throw new ArgumentOutOfRangeException(nameof(feature)),
            };
        }

        public async Task SetCoordinates(MapFeatureDef feature, string datasourceId)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, feature.Id, feature.Coordinates, datasourceId);
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => ModuleExtensions.GetJsModuleMethod(JsModule.Features, name);
    }
}
