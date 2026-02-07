using Marqdouj.DotNet.AzureMaps.Blazor.Models.Configuration;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop.Modules
{
    public interface IAzMapConfigurations
    {
        Task<CameraOptionsGet> GetCamera();
        Task<MapOptionsGet> GetMapOptions();
        Task<ServiceOptions> GetServiceOptions();
        Task<StyleOptions> GetStyle();
        Task<TrafficOptions> GetTraffic();
        Task<UserInteractionOptions> GetUserInteraction();
        Task SetCamera(CameraOptions? camera, CameraBoundsOptionsSet? cameraBounds = null, AnimationOptions? animation = null);
        Task SetMapOptions(MapOptionsSet options);
        Task SetServiceOptions(ServiceOptions options);
        Task SetStyle(StyleOptions options);
        Task SetTraffic(TrafficOptions? options);
        Task SetUserInteraction(UserInteractionOptions options);
        Task ZoomTo(Position center, double zoomLevel);
    }

    internal class AzConfigurations(IAzMapModuleReferences dotNetRef, string mapId) : IAzMapConfigurations
    {
        private readonly IAzMapModuleReferences dotNetRef = dotNetRef;
        private readonly string mapId = mapId;
        private IJSRuntime JsRuntime => dotNetRef.JsRuntime;

        #region Camera
        public async Task<CameraOptionsGet> GetCamera()
        {
            return await JsRuntime.InvokeAsync<CameraOptionsGet>(GetJsInteropMethod(), mapId);
        }

        public async Task SetCamera(CameraOptions? camera, CameraBoundsOptionsSet? cameraBounds = null, AnimationOptions? animation = null)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, new CameraOptionsSet(camera, cameraBounds, animation));
        }

        public async Task ZoomTo(Position center, double zoomLevel)
        {
            var camera = await GetCamera();
            var options = camera.ToCameraOptions();
            options.Center = center;
            options.Zoom = zoomLevel;
            await SetCamera(options);
        }
        #endregion

        #region MapOptions
        public async Task SetMapOptions(MapOptionsSet options)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, options);
        }

        public async Task<MapOptionsGet> GetMapOptions()
        {
            return await JsRuntime.InvokeAsync<MapOptionsGet>(GetJsInteropMethod(), mapId);
        }
        #endregion

        #region Service
        public async Task<ServiceOptions> GetServiceOptions()
        {
            return await JsRuntime.InvokeAsync<ServiceOptions>(GetJsInteropMethod(), mapId);
        }

        public async Task SetServiceOptions(ServiceOptions options)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, options);
        }
        #endregion

        #region Style
        public async Task<StyleOptions> GetStyle()
        {
            return await JsRuntime.InvokeAsync<StyleOptions>(GetJsInteropMethod(), mapId);
        }

        public async Task SetStyle(StyleOptions options)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, options);
        }
        #endregion

        #region Traffic
        public async Task<TrafficOptions> GetTraffic()
        {
            return await JsRuntime.InvokeAsync<TrafficOptions>(GetJsInteropMethod(), mapId);
        }

        public async Task SetTraffic(TrafficOptions? options)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, options);
        }
        #endregion

        #region UserInteraction
        public async Task<UserInteractionOptions> GetUserInteraction()
        {
            return await JsRuntime.InvokeAsync<UserInteractionOptions>(GetJsInteropMethod(), mapId);
        }

        public async Task SetUserInteraction(UserInteractionOptions options)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, options);
        }
        #endregion

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => ModuleExtensions.GetJsModuleMethod(JsModule.Configurations, name);
    }
}
