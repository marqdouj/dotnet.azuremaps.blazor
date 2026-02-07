using Microsoft.JSInterop;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop
{
    internal interface IAzMapModuleReferences
    {
        object DotNetRef { get; }
        IJSRuntime JsRuntime { get; }
    }

    internal class AzMapModuleReferences(IJSRuntime jsRuntime, object dotNetRef) : IAzMapModuleReferences
    {
        public IJSRuntime JsRuntime { get; } = jsRuntime;
        public object DotNetRef { get; } = dotNetRef;
    }
}