using Marqdouj.DotNet.AzureMaps.Blazor.Models.Events;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop.Modules
{
    public interface IAzMapLayers
    {
        Task Add(IEnumerable<MapLayerDef> items, IEnumerable<MapEventDef>? events = null);
        Task Add(MapLayerDef item, IEnumerable<MapEventDef>? events = null);
        Task AddGroup(LayerEventsGroup item);
        Task AddGroups(IEnumerable<LayerEventsGroup> items);
        Task<LayerOptions?> GetOptions(MapLayerDef layerDef);
        Task Remove(IEnumerable<MapLayerDef> items);
        Task Remove(MapLayerDef item);
        Task RemoveById(IEnumerable<string> layers);
        Task SetOptions(MapLayerDef layerDef);
    }

    internal class AzLayers(IAzMapModuleReferences dotNetRef, string mapId) : IAzMapLayers
    {
        private readonly IAzMapModuleReferences dotNetRef = dotNetRef;
        private readonly string mapId = mapId;
        private IJSRuntime JsRuntime => dotNetRef.JsRuntime;

        public async Task Add(MapLayerDef item, IEnumerable<MapEventDef>? events = null)
        {
            await Add([item], events);
        }

        public async Task Add(IEnumerable<MapLayerDef> items, IEnumerable<MapEventDef>? events = null)
        {
            ValidateMapLayerDefs(items);
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, items?.Cast<object>().ToList(), events?.Cast<object>().ToList());
        }

        public async Task AddGroup(LayerEventsGroup item)
        {
            await AddGroups([item]);
        }

        public async Task AddGroups(IEnumerable<LayerEventsGroup> items)
        {
            ValidateMapLayerDefs(items.Select(e => e.Layer));
            var groups = items.Select(e => new { Layer = (object)e.Layer, Events = (object)e.Events }).ToList();
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, groups);
        }

        private static void ValidateMapLayerDefs(IEnumerable<MapLayerDef> items)
        {
            foreach (var item in items)
            {
                ArgumentNullException.ThrowIfNull(item, nameof(MapEventDef));
                ArgumentNullException.ThrowIfNull(item.DataSource, nameof(item.DataSource));
            }
        }

        public async Task Remove(MapLayerDef item)
        {
            await Remove([item]);
        }

        public async Task Remove(IEnumerable<MapLayerDef> items)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, items?.Cast<object>().ToList());
        }

        public async Task RemoveById(IEnumerable<string> layers)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, layers);
        }

        public async Task<LayerOptions?> GetOptions(MapLayerDef layerDef)
        {
            var layerId = layerDef.JsInterop.Id;

            return layerDef.LayerType switch
            {
                MapLayerType.Bubble => await JsRuntime.InvokeAsync<BubbleLayerOptions>(GetJsInteropMethod(), mapId, layerId),
                MapLayerType.HeatMap => await JsRuntime.InvokeAsync<HeatMapLayerOptions>(GetJsInteropMethod(), mapId, layerId),
                MapLayerType.Image => await JsRuntime.InvokeAsync<ImageLayerOptions>(GetJsInteropMethod(), mapId, layerId),
                MapLayerType.Line => await JsRuntime.InvokeAsync<LineLayerOptions>(GetJsInteropMethod(), mapId, layerId),
                MapLayerType.Polygon => await JsRuntime.InvokeAsync<PolygonLayerOptions>(GetJsInteropMethod(), mapId, layerId),
                MapLayerType.PolygonExtrusion => await JsRuntime.InvokeAsync<PolygonExtLayerOptions>(GetJsInteropMethod(), mapId, layerId),
                MapLayerType.Symbol => await JsRuntime.InvokeAsync<SymbolLayerOptions>(GetJsInteropMethod(), mapId, layerId),
                MapLayerType.Tile => await JsRuntime.InvokeAsync<TileLayerOptions>(GetJsInteropMethod(), mapId, layerId),
                _ => null,
            };
        }

        public async Task SetOptions(MapLayerDef layerDef)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, layerDef);
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => ModuleExtensions.GetJsModuleMethod(JsModule.Layers, name);
    }
}
