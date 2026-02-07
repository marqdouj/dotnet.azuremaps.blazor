using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Controls
{
    public class TrafficLegendControl : MapControl
    {
        public TrafficLegendControl(MapControlPosition? position = null, TrafficLegendControlOptions? options = null)
        {
            Type = MapControlType.TrafficLegend;
            ControlOptions = new MapControlOptions();
            if (position != null)
                ControlOptions.Position = position;
            Options = options;
            SortOrder = 0;
        }

        public TrafficLegendControlOptions? Options { get; set; }

        /// <summary>
        /// Makes a copy of this control
        /// </summary>
        /// <param name="fullCopy">if true, copies the internal settings</param>
        /// <returns></returns>
        public TrafficLegendControl Copy(bool fullCopy = true)
        {
            var control = new TrafficLegendControl
            {
                ControlOptions = (MapControlOptions?)ControlOptions?.Clone(),
                Options = (TrafficLegendControlOptions?)Options?.Clone(),
                SortOrder = SortOrder
            };

            if (fullCopy)
            {
                control.JsInterop.Id = JsInterop.Id;
            }

            return control;
        }
    }

    public class TrafficLegendControlOptions : ICloneable
    {
        public bool? IsActive { get; set; }

        /// <summary>
        /// The style of the control.
        /// Default 'Light'.
        /// </summary>
        [JsonIgnore]
        public MapControlStyle? Style { get; set; }

        [JsonInclude]
        [JsonPropertyName("style")]
        internal string? StyleJs { get => Style.EnumToJsonN(); set => Style = value.JsonToEnumN<MapControlStyle>(); }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
