using Marqdouj.DotNet.AzureMaps.Blazor.Models.Events;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop.Modules
{
    public interface IAzMapSources
    {
        Task Add(IEnumerable<SourceDef> items);
        Task Add(SourceDef item);
        Task Clear(IEnumerable<SourceDef> items);
        Task Clear(SourceDef item);
        Task ClearById(IEnumerable<string> sourceIds);
        Task<List<MapEventShape>> GetShapes(string sourceId);
        Task Remove(IEnumerable<SourceDef> items);
        Task Remove(SourceDef item);
        Task RemoveById(IEnumerable<string> sourceIds);
    }

    internal class AzSources(IAzMapModuleReferences dotNetRef, string mapId) : IAzMapSources
    {
        private readonly IAzMapModuleReferences dotNetRef = dotNetRef;
        private readonly string mapId = mapId;
        private IJSRuntime JsRuntime => dotNetRef.JsRuntime;

        public async Task Add(SourceDef item)
        {
            await Add([item]);
        }

        public async Task Remove(SourceDef item)
        {
            await Remove([item]);
        }

        public async Task Clear(SourceDef item)
        {
            await Clear([item]);
        }

        public async Task Add(IEnumerable<SourceDef> items)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, items?.Cast<object>().ToList());
        }

        public async Task Remove(IEnumerable<SourceDef> items)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, items?.Cast<object>().ToList());
        }

        public async Task RemoveById(IEnumerable<string> sourceIds)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, sourceIds);
        }

        public async Task Clear(IEnumerable<SourceDef> items)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, items?.Cast<object>().ToList());
        }

        public async Task ClearById(IEnumerable<string> sourceIds)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, sourceIds);
        }

        public async Task<List<MapEventShape>> GetShapes(string sourceId)
        {
            return await JsRuntime.InvokeAsync<List<MapEventShape>>(GetJsInteropMethod(), mapId, sourceId);
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => ModuleExtensions.GetJsModuleMethod(JsModule.Sources, name);
    }
}
