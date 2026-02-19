namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson
{
    /// <summary>
    /// <![CDATA[FeatureCollection<G, P>]]> — collection of features.
    /// <see href="https://datatracker.ietf.org/doc/html/rfc7946#section-3.3"/>
    /// </summary>
    public sealed class FeatureCollection<G, P> : GeoJsonObject where G : Geometry
    {
        private readonly List<Feature<G, P>> features;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="features"><see cref="Feature{G, P}"/></param>
        /// <param name="bbox"><see cref="BoundingBox"/></param>
        /// <exception cref="ArgumentException"></exception>
        public FeatureCollection(IEnumerable<Feature<G, P>> features, BoundingBox? bbox = null) : base(GeoJsonType.FeatureCollection)
        {
            ArgumentNullException.ThrowIfNull(features);
            this.features = [.. features];
            if (this.features.Count == 0)
                throw new ArgumentException("FeatureCollection must contain at least one Feature.", nameof(features));
            BBox = bbox;
        }

        /// <summary>
        /// Readonly list of features.
        /// </summary>
        public IReadOnlyList<Feature<G, P>> Features => features.AsReadOnly();
    }
}
