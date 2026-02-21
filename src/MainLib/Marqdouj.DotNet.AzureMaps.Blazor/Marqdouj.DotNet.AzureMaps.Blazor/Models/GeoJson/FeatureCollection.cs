using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson
{
    /// <summary>
    /// <![CDATA[FeatureCollection<G, P>]]> — collection of features.
    /// <see href="https://datatracker.ietf.org/doc/html/rfc7946#section-3.3"/>
    /// </summary>
    public class FeatureCollection<G, P> : GeoJsonObject where G : Geometry
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonConstructor]
        public FeatureCollection() : base(GeoJsonType.FeatureCollection)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="features"><see cref="Feature{G, P}"/></param>
        /// <param name="bbox"><see cref="BoundingBox"/></param>
        /// <exception cref="ArgumentException"></exception>
        public FeatureCollection(IEnumerable<Feature<G, P>>? features = null, BoundingBox? bbox = null) : base(GeoJsonType.FeatureCollection)
        {
            Features = features?.ToList();
            Bbox = bbox;
        }

        /// <summary>
        /// List of features.
        /// </summary>
        public List<Feature<G, P>>? Features { get; set; }
    }
}
