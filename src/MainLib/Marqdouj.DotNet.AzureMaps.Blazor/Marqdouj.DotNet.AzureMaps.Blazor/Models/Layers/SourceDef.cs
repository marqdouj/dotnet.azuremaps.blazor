using Marqdouj.DotNet.AzureMaps.Blazor.Models.Common;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers
{
    public enum SourceType
    {
        DataSource,
        //ElevationTile,
        //VectorTile,
    }

    public abstract class SourceDef : JsInteropBase
    {
        /// <summary>
        /// The unique identifier for the source.
        /// </summary>
        public override string Id { get; set { if (string.IsNullOrWhiteSpace(value)) return; field = value.TrimCssId(); } } = AzMapsExtensions.GetRandomCssId();

        [JsonIgnore]
        public SourceType? Type { get; internal set; }

        [JsonInclude]
        [JsonPropertyName("type")]
        internal string? TypeJs { get => Type.ToString(); set => Type = value.JsonToEnumN<SourceType>(); }
    }
}
