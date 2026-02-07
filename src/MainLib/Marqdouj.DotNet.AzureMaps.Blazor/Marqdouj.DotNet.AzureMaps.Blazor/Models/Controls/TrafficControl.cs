using Marqdouj.DotNet.AzureMaps.Blazor.Models.Configuration;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Controls
{
    public class TrafficControl : MapControl
    {
        public TrafficControl(MapControlPosition? position = null, TrafficControlOptions? options = null)
        {
            Type = MapControlType.Traffic;
            ControlOptions = new MapControlOptions();
            if (position != null)
                ControlOptions.Position = position;
            Options = options;
            SortOrder = 0;
        }

        public TrafficControlOptions? Options { get; set; }

        /// <summary>
        /// Makes a copy of this control
        /// </summary>
        /// <param name="fullCopy">if true, copies the internal settings</param>
        /// <returns></returns>
        public TrafficControl Copy(bool fullCopy = true)
        {
            var control = new TrafficControl
            {
                ControlOptions = (MapControlOptions?)ControlOptions?.Clone(),
                Options = (TrafficControlOptions?)Options?.Clone(),
                SortOrder = SortOrder
            };

            if (fullCopy)
            {
                control.JsInterop.Id = JsInterop.Id;
            }

            return control;
        }
    }

    public class TrafficControlOptions : TrafficOptions, ICloneable
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
