using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Events
{
    public class MapEventKeyboardPayload
    {
        [JsonInclude] public string? Key { get; internal set; }
        [JsonInclude] public string? Code { get; internal set; }
        [JsonInclude] public long? Location { get; internal set; }
        [JsonInclude] public bool? Repeat { get; internal set; }
        [JsonInclude] public bool? AltKey { get; internal set; }
        [JsonInclude] public bool? CtrlKey { get; internal set; }
        [JsonInclude] public bool? ShiftKey { get; internal set; }
        [JsonInclude] public bool? MetaKey { get; internal set; }
    }
}
