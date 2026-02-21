namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson
{
    /// <summary>
    /// <![CDATA[Feature<G, P>]]> — generic Feature object.
    /// G represents the geometry type (subclass of Geometry). Geometry MAY be null for a Feature.
    /// P represents properties (dictionary mapping string to any) and can be null.
    /// <see href="https://datatracker.ietf.org/doc/html/rfc7946#section-3.2"/>
    /// </summary>
    public class Feature<G, P> : GeoJsonObject where G : Geometry
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"><see cref="GeoJson.Geometry"/></param>
        /// <param name="properties"><![CDATA[Dictionary<string, object?>]]></param>
        /// <param name="id"><see cref="Id"/></param>
        /// <param name="bbox"><see cref="BoundingBox"/></param>
        public Feature(G? geometry = null, P? properties = default, object? id = null, BoundingBox? bbox = null) : base(GeoJsonType.Feature)
        {
            Geometry = geometry;
            Properties = properties;
            Id = id;
            Bbox = bbox;
        }

        /// <summary>
        /// The geometry may be null (e.g., Feature with null geometry).
        /// </summary>
        public G? Geometry { get; set; }

        /// <summary>
        /// Id can be string or number in original spec; we model as object? (string, int, long, double, or null).
        /// </summary>
        public object? Id { get; set; }

        /// <summary>
        /// Properties: often a dictionary, but generic type P allows custom property containers.
        /// The original type allowed { [name: string]: any } | null. Typical use: <![CDATA[IDictionary<string, object?]]>.
        /// </summary>
        public P? Properties { get; set; }
    }
}
