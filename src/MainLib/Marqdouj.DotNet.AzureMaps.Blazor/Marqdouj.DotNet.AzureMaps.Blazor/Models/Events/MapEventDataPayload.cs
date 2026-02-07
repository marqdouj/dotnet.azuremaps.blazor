using Marqdouj.DotNet.AzureMaps.Blazor.Models.Common;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Events
{
    public enum MapEventDataPayloadDataType
    {
        Style,
        Source
    }

    public enum MapEventDataPayloadSourceDataType
    {
        Metadata,
        Content
    }

    public class MapEventDataPayload
    {
        [JsonIgnore]
        public MapEventDataPayloadDataType? DataType => DataTypeJs.JsonToEnumN<MapEventDataPayloadDataType>();

        [JsonInclude]
        [JsonPropertyName("dataType")]
        internal string? DataTypeJs { get; set; }

        public bool? IsSourceLoaded { get; set; }
        public string? Source { get; set; }

        [JsonIgnore]
        public MapEventDataPayloadSourceDataType? SourceDataType => SourceDataTypeJs.JsonToEnumN<MapEventDataPayloadSourceDataType>();

        [JsonInclude]
        [JsonPropertyName("sourceDataType")]
        internal string? SourceDataTypeJs { get; set; }

        public Tile? Tile { get; set; }
    }
}
