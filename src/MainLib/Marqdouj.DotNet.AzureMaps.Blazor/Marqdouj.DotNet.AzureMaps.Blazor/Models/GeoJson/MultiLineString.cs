namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson
{
    public class MultiLineString() : Geometry(GeometryType.MultiLineString)
    {
        private List<List<Position>> coordinates = [];

        public BoundingBox? Bbox { get; set; }
        public List<List<Position>> Coordinates { get => coordinates; set => coordinates = value ?? []; }
    }
}
