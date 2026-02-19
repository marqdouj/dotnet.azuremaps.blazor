namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson
{
    public class MultiPoint() : Geometry(GeometryType.MultiPoint)
    {
        private List<Position> coordinates = [];

        public MultiPoint(List<Position> coordinates) : this() => Coordinates = coordinates;

        public List<Position> Coordinates { get => coordinates; set => coordinates = value ?? []; }
    }
}
