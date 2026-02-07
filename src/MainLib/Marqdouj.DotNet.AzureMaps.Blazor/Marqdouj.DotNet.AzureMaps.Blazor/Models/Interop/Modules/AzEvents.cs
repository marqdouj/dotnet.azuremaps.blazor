using Marqdouj.DotNet.AzureMaps.Blazor.Models.Events;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop.Modules
{
    public interface IAzMapEvents
    {
        Task Add(IEnumerable<MapEventDef> items);
        Task Add(MapEventDef item);
        Task Remove(IEnumerable<MapEventDef> items);
        Task Remove(MapEventDef item);
    }

    internal class AzEvents(IAzMapModuleReferences dotNetRef, string mapId) : IAzMapEvents
    {
        private readonly IAzMapModuleReferences dotNetRef = dotNetRef;
        private readonly string mapId = mapId;
        private IJSRuntime JsRuntime => dotNetRef.JsRuntime;

        public async Task Add(MapEventDef item)
        {
            await Add([item]);
        }

        public async Task Remove(MapEventDef item)
        {
            await Remove([item]);
        }

        public async Task Add(IEnumerable<MapEventDef> items)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, items?.Cast<object>().ToList());
        }

        public async Task Remove(IEnumerable<MapEventDef> items)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, items?.Cast<object>().ToList());
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => ModuleExtensions.GetJsModuleMethod(JsModule.Events, name);
    }
}
