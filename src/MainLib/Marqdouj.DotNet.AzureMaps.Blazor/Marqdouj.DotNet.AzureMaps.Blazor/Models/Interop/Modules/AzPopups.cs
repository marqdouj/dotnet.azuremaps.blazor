using Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop.Modules
{
    public interface IAzMapPopups
    {
        Task Add(IEnumerable<PopupDef> items);
        Task Add(PopupDef item);
        Task Remove(IEnumerable<PopupDef> items);
        Task Remove(PopupDef item);
    }

    internal class AzPopups(IAzMapModuleReferences dotNetRef, string mapId) : IAzMapPopups
    {
        private readonly IAzMapModuleReferences dotNetRef = dotNetRef;
        private readonly string mapId = mapId;
        private IJSRuntime JsRuntime => dotNetRef.JsRuntime;

        public async Task Add(PopupDef item)
        {
            await Add([item]);
        }
        public async Task Remove(PopupDef item)
        {
            await Remove([item]);
        }

        public async Task Add(IEnumerable<PopupDef> items)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, items?.Cast<object>().ToList());
        }

        public async Task Remove(IEnumerable<PopupDef> items)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, items?.Cast<object>().ToList());
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => ModuleExtensions.GetJsModuleMethod(JsModule.Popups, name);
    }
}
