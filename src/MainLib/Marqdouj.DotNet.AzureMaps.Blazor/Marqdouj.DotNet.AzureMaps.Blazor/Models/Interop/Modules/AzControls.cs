using Marqdouj.DotNet.AzureMaps.Blazor.Models.Controls;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop.Modules
{
    public interface IAzMapControls
    {
        Task Add(IEnumerable<MapControl> items);
        Task Add(MapControl item);
        Task Remove(IEnumerable<MapControl> items);
        Task Remove(MapControl item);
    }

    internal class AzControls(IAzMapModuleReferences dotNetRef, string mapId) : IAzMapControls
    {
        private readonly IAzMapModuleReferences dotNetRef = dotNetRef;
        private readonly string mapId = mapId;
        private IJSRuntime JsRuntime => dotNetRef.JsRuntime;

        public async Task Add(MapControl item)
        {
            await Add([item]);
        }

        public async Task Add(IEnumerable<MapControl> items)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, items?.Cast<object>().ToList());
        }

        public async Task Remove(MapControl item)
        {
            await Remove([item]);
        }

        public async Task Remove(IEnumerable<MapControl> items)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, items?.Cast<object>().ToList());
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => ModuleExtensions.GetJsModuleMethod(JsModule.Controls, name);
    }
}
