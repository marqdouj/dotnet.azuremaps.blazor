using Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop.Modules;
using Microsoft.JSInterop;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Utilities
{
    public class Mercators(IJSRuntime jsRuntime) : IAzMapMercators
    {
        private readonly AzMercators azMercators = new(jsRuntime);

        public async Task<MercatorPoint> FromPosition(Position position)
        {
            return await ((IAzMapMercators)azMercators).FromPosition(position);
        }

        public async Task<List<MercatorPoint>> FromPositions(IEnumerable<Position> positions)
        {
            return await ((IAzMapMercators)azMercators).FromPositions(positions);
        }

        public async Task<double> MercatorScale(double latitude)
        {
            return await ((IAzMapMercators)azMercators).MercatorScale(latitude);
        }

        public async Task<double> MeterInMercatorUnits(double latitude)
        {
            return await ((IAzMapMercators)azMercators).MeterInMercatorUnits(latitude);
        }

        public async Task<List<float>> ToFloat32Array(List<Position> positions)
        {
            return await ((IAzMapMercators)azMercators).ToFloat32Array(positions);
        }

        public async Task<Position> ToPosition(MercatorPoint mercator)
        {
            return await ((IAzMapMercators)azMercators).ToPosition(mercator);
        }

        public async Task<List<Position>> ToPositions(IEnumerable<MercatorPoint> mercators)
        {
            return await ((IAzMapMercators)azMercators).ToPositions(mercators);
        }
    }
}
