using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Events
{
    public class MapEventPopupPayload
    {
        [JsonInclude]
        public string? Type { get; internal set; }
    }
}
