using Marqdouj.DotNet.AzureMaps.Blazor.Models.Common;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Events
{
    public class MapEventArgsPayload
    {
        [JsonInclude]
        public JSInteropDef? JsInterop { get; internal set; }
        public MapEventConfigPayload? Config { get; set; }
        public MapEventDataPayload? Data { get; set; }
        public MapEventDataSourcePayload? DataSource { get; set; }
        public MapEventErrorPayload? Error { get; set; }
        public MapEventHtmlMarkerPayload? HtmlMarker { get; set; }
        public MapEventLayerPayload? Layer { get; set; }
        public MapEventMousePayload? Mouse { get; set; }
        public MapEventPopupPayload? Popup { get; set; }
        public MapEventSourcePayload? Source { get; set; }
        public MapEventStylePayload? Style { get; set; }
        public MapEventStyleControlPayload? StyleControl { get; set; }
        public MapEventTouchPayload? Touch { get; set; }
        public MapEventWheelPayload? Wheel { get; set; }
    }
}
