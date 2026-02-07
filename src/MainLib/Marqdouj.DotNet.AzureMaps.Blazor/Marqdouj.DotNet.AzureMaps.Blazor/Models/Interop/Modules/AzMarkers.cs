using Marqdouj.DotNet.AzureMaps.Blazor.Models.Events;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop.Modules
{
    public interface IAzMapMarkers
    {
        Task Add(IEnumerable<HtmlMarkerDef> items, IEnumerable<MapEventDef>? events = null);
        Task Add(HtmlMarkerDef item, IEnumerable<MapEventDef>? events = null);
        Task Remove(IEnumerable<HtmlMarkerDef> items);
        Task Remove(HtmlMarkerDef item);
    }

    internal class AzMarkers(IAzMapModuleReferences dotNetRef, string mapId) : IAzMapMarkers
    {
        private readonly IAzMapModuleReferences dotNetRef = dotNetRef;
        private readonly string mapId = mapId;
        private IJSRuntime JsRuntime => dotNetRef.JsRuntime;

        public async Task Add(HtmlMarkerDef item, IEnumerable<MapEventDef>? events = null)
        {
            await Add([item], events);
        }

        public async Task Add(IEnumerable<HtmlMarkerDef> items, IEnumerable<MapEventDef>? events = null)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, items?.Cast<object>().ToList(), events?.Cast<object>().ToList());
        }

        public async Task Remove(HtmlMarkerDef item)
        {
            await Remove([item]);
        }

        public async Task Remove(IEnumerable<HtmlMarkerDef> items)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, items?.Cast<object>().ToList());
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => ModuleExtensions.GetJsModuleMethod(JsModule.Markers, name);
    }
}
