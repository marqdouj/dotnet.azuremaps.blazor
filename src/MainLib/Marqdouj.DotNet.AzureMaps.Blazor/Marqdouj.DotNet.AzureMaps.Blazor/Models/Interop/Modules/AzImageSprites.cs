using Marqdouj.DotNet.AzureMaps.Blazor.Models.Images;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop.Modules
{
    public interface IAzMapImageSprites
    {
        Task<bool> Add(string id, ImageData icon, StyleImageMetadata? meta);
        Task<bool> Add(string id, string icon, StyleImageMetadata? meta);
        Task Clear();
        Task<bool> CreateFromTemplate(ImageTemplateDef templateDef);
        Task<List<string>> GetImageIds();
        Task<bool> HasImage(ImageTemplateDef templateDef);
        Task<bool> HasImage(string id);
        Task Remove(string id);
    }

    internal class AzImageSprites(IAzMapModuleReferences dotNetRef, string mapId) : IAzMapImageSprites
    {
        private readonly IAzMapModuleReferences dotNetRef = dotNetRef;
        private readonly string mapId = mapId;
        private IJSRuntime JsRuntime => dotNetRef.JsRuntime;

        public async Task<bool> CreateFromTemplate(ImageTemplateDef templateDef)
        {
            return await JsRuntime.InvokeAsync<bool>(GetJsInteropMethod(), mapId, templateDef);
        }

        public async Task<bool> HasImage(ImageTemplateDef templateDef)
        {
            return await JsRuntime.InvokeAsync<bool>(GetJsInteropMethod(), mapId, templateDef.Id);
        }

        public async Task<bool> HasImage(string id)
        {
            return await JsRuntime.InvokeAsync<bool>(GetJsInteropMethod(), mapId, id);
        }

        public async Task<bool> Add(string id, string icon, StyleImageMetadata? meta)
        {
            return await JsRuntime.InvokeAsync<bool>(GetJsInteropMethod(), mapId, id, icon, meta);
        }

        public async Task<bool> Add(string id, ImageData icon, StyleImageMetadata? meta)
        {
            return await JsRuntime.InvokeAsync<bool>(GetJsInteropMethod(), mapId, id, icon, meta);
        }

        public async Task Clear()
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId);
        }

        public async Task<List<string>> GetImageIds()
        {
            return await JsRuntime.InvokeAsync<List<string>>(GetJsInteropMethod(), mapId);
        }

        public async Task Remove(string id)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), mapId, id);
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => ModuleExtensions.GetJsModuleMethod(JsModule.ImageSprites, name);
    }
}
