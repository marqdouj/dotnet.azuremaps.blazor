namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson
{
    public class Polygon() : Geometry(GeometryType.Polygon)
    {
        private List<List<Position>> coordinates = [];

        public Polygon(List<List<Position>> coordinates) : this() => Coordinates = coordinates;

        public List<List<Position>> Coordinates { get => coordinates; set => coordinates = value ?? []; }
    }
}
