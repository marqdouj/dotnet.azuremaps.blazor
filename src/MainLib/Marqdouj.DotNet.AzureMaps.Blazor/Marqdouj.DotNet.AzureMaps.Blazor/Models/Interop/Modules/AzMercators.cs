using Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop.Modules
{
    public interface IAzMapMercators
    {
        Task<MercatorPoint> FromPosition(Position position);
        Task<List<MercatorPoint>> FromPositions(IEnumerable<Position> positions);
        Task<double> MercatorScale(double latitude);
        Task<double> MeterInMercatorUnits(double latitude);
        Task<List<float>> ToFloat32Array(List<Position> positions);
        Task<Position> ToPosition(MercatorPoint mercator);
        Task<List<Position>> ToPositions(IEnumerable<MercatorPoint> mercators);
    }

    internal class AzMercators(IJSRuntime jsRuntime) : IAzMapMercators
    {
        private readonly IJSRuntime JsRuntime = jsRuntime;

        #region FromPosition
        public async Task<MercatorPoint> FromPosition(Position position)
        {
            return await JsRuntime.InvokeAsync<MercatorPoint>(GetJsInteropMethod(), position);
        }

        public async Task<List<MercatorPoint>> FromPositions(IEnumerable<Position> positions)
        {
            return await JsRuntime.InvokeAsync<List<MercatorPoint>>(GetJsInteropMethod(), positions);
        }
        #endregion

        #region ToPosition
        public async Task<Position> ToPosition(MercatorPoint mercator)
        {
            return await JsRuntime.InvokeAsync<Position>(GetJsInteropMethod(), mercator);
        }

        public async Task<List<Position>> ToPositions(IEnumerable<MercatorPoint> mercators)
        {
            return await JsRuntime.InvokeAsync<List<Position>>(GetJsInteropMethod(), mercators);
        }
        #endregion

        public async Task<List<float>> ToFloat32Array(List<Position> positions)
        {
            return await JsRuntime.InvokeAsync<List<float>>(GetJsInteropMethod(), positions);
        }

        public async Task<double> MercatorScale(double latitude)
        {
            return await JsRuntime.InvokeAsync<double>(GetJsInteropMethod(), latitude);
        }

        public async Task<double> MeterInMercatorUnits(double latitude)
        {
            return await JsRuntime.InvokeAsync<double>(GetJsInteropMethod(), latitude);
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => ModuleExtensions.GetJsModuleMethod(JsModule.Mercators, name);
    }
}
