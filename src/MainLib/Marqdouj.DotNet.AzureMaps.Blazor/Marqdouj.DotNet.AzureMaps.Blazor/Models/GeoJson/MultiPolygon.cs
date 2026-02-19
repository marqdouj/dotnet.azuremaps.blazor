namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson
{
    public class MultiPolygon() : Geometry(GeometryType.MultiPolygon)
    {
        private List<List<List<Position>>> coordinates = [];

        public List<List<List<Position>>> Coordinates { get => coordinates; set => coordinates = value ?? []; }
    }
}
