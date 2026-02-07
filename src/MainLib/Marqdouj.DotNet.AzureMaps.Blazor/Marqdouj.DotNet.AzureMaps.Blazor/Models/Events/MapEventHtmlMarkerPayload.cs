using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Events
{
    public class MapEventHtmlMarkerPayload
    {
        [JsonInclude]
        public string? Type { get; internal set; }

        public MapEventKeyboardPayload? Keyboard { get; set; }
    }
}
