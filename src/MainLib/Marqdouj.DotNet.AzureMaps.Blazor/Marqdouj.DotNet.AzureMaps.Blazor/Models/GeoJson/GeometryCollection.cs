namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson
{
    /// <summary>
    /// GeometryCollection: List of Geometry objects.
    /// <see href="https://datatracker.ietf.org/doc/html/rfc7946#section-3.1.8"/>
    /// </summary>
    public class GeometryCollection : Geometry
    {
        private readonly List<Geometry> geometries;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometries"><see cref="Geometry"/></param>
        /// <param name="bbox"><see cref="BoundingBox"/></param>
        /// <exception cref="ArgumentException"></exception>
        public GeometryCollection(IEnumerable<Geometry> geometries, BoundingBox? bbox = null) : base(GeometryType.GeometryCollection)
        {
            ArgumentNullException.ThrowIfNull(geometries);
            this.geometries = [.. geometries];
            if (this.geometries.Count == 0)
                throw new ArgumentException("GeometryCollection must contain at least one Geometry.", nameof(geometries));
            BBox = bbox;
        }

        /// <summary>
        /// Readonly List of geometries.
        /// </summary>
        public IReadOnlyList<Geometry> Geometries => geometries.AsReadOnly();
    }
}
