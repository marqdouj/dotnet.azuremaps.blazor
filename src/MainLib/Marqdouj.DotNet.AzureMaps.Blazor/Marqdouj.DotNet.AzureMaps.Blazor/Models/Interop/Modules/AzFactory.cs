using Marqdouj.DotNet.AzureMaps.Blazor.Models.Configuration;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Controls;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Events;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop.Modules
{
    internal class AzFactory(IAzMapModuleReferences dotNetRef, MapAuthentication authentication, string mapId, LogLevel logLevel)
    {
        private readonly IAzMapModuleReferences dotNetRef = dotNetRef;
        private readonly MapAuthentication authentication = authentication;
        private readonly string mapId = mapId;
        private readonly LogLevel logLevel = logLevel;

        private IJSRuntime JsRuntime => dotNetRef.JsRuntime;
        
        public async Task CreateMap(
            IEnumerable<MapEventDef>? events,
            MapOptions? options,
            IEnumerable<MapControl>? controls)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), 
                new { 
                    dotNetRef.DotNetRef, 
                    MapId = mapId, 
                    AuthOptions = authentication, 
                    Events = events, 
                    Options = options, 
                    Controls = controls?.Cast<object>().ToList(),
                    LogLevel = logLevel });
        }

        public async Task RemoveMap()
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId);
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => ModuleExtensions.GetJsModuleMethod(JsModule.MapFactory, name);
    }
}
