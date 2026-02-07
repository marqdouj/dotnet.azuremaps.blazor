using Marqdouj.DotNet.AzureMaps.Blazor.Models.Common;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers
{
    /// <summary>
    /// defines a layer to be added to the map.
    /// </summary>
    public abstract class MapLayerDef : JsInteropBase
    {
        protected internal MapLayerDef() { }

        /// <summary>
        /// The unique identifier for the layer.
        /// </summary>
        public override string Id { get; set { if (string.IsNullOrWhiteSpace(value)) return; field = value.TrimCssId(); } } = AzMapsExtensions.GetRandomCssId();

        /// <summary>
        /// The type of layer.
        /// </summary>
        [JsonIgnore]
        public abstract MapLayerType LayerType { get; }

        [JsonInclude]
        [JsonPropertyName("type")]
        internal string Type { get => LayerType.ToString(); }

        /// <summary>
        ///  Optionally specify a layer id to insert the new layer(s) before it.
        ///  Specify "labels" to place the new layer(s) just below the default label layer,
        ///  which will allow the labels to be visible on top of the custom layer.
        /// </summary>
        public string? Before { get; set; }

        /// <summary>
        /// Gets or sets the configuration options for the data source.
        /// </summary>
        public DataSourceDef DataSource { get; set { ArgumentNullException.ThrowIfNull(field, nameof(DataSource)); field = value; } } = new();
    }
}
