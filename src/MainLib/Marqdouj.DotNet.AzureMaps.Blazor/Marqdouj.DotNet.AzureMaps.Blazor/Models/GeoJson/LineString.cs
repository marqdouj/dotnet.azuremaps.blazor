using Marqdouj.DotNet.AzureMaps.Blazor.Models.Common;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson
{
    public class LineString() : Geometry(GeometryType.LineString)
    {
        private List<Position> coordinates = [];

        public LineString(List<Position> coordinates) : this() => Coordinates = coordinates;

        public BoundingBox? Bbox { get; set; }

        public List<Position> Coordinates { get => coordinates; set => coordinates = value ?? []; }
    }
}
