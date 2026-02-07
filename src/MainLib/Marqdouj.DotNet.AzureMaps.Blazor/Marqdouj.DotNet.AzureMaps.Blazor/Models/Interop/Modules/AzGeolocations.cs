using Marqdouj.DotNet.AzureMaps.Blazor.Models.Geolocation;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop.Modules
{
    public interface IAzMapGeolocations
    {
        Task ClearWatch();
        Task<GeolocationResult> GetLocation(PositionOptions? options);
        Task<bool> IsWatched();
        Task<int?> WatchPosition(PositionOptions? options = null);
    }

    internal class AzGeolocations(IAzMapModuleReferences dotNetRef, string mapId) : IAzMapGeolocations
    {
        private readonly IAzMapModuleReferences dotNetRef = dotNetRef;
        private readonly string mapId = mapId;
        private IJSRuntime JsRuntime => dotNetRef.JsRuntime;

        public async Task<GeolocationResult> GetLocation(PositionOptions? options)
        {
            return await JsRuntime.InvokeAsync<GeolocationResult>(GetJsInteropMethod(), options);
        }

        public async Task<int?> WatchPosition(PositionOptions? options = null)
        {
            return await JsRuntime.InvokeAsync<int?>(GetJsInteropMethod(), dotNetRef.DotNetRef, mapId, options);
        }

        public async Task ClearWatch()
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId);
        }

        public async Task<bool> IsWatched()
        {
            return await JsRuntime.InvokeAsync<bool>(GetJsInteropMethod(), mapId);
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => ModuleExtensions.GetJsModuleMethod(JsModule.Geolocations, name);
    }
}
