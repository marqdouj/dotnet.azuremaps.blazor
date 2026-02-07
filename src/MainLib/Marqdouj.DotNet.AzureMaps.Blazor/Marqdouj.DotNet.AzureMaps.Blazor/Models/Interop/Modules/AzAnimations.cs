using Marqdouj.DotNet.AzureMaps.Blazor.Models.Animations;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop.Modules
{
    public interface IAzMapAnimations
    {
        Task AnimateShape(ShapeAnimationOptions options);
        Task<List<string>> GetEasingNames();
    }

    internal class AzAnimations(IAzMapModuleReferences dotNetRef, string mapId) : IAzMapAnimations
    {
        private readonly IAzMapModuleReferences dotNetRef = dotNetRef;
        private readonly string mapId = mapId;
        private IJSRuntime JsRuntime => dotNetRef.JsRuntime;

        public async Task<List<string>> GetEasingNames()
        {
            return await JsRuntime.InvokeAsync<List<string>>(GetJsInteropMethod(), mapId);
        }

        public async Task AnimateShape(ShapeAnimationOptions options)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, options.SerializeForJsInterop());
        }


        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => ModuleExtensions.GetJsModuleMethod(JsModule.Animations, name);
    }
}
