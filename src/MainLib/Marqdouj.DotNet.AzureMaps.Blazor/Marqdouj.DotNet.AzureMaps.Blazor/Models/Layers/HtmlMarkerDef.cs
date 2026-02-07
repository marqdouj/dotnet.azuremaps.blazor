using Marqdouj.DotNet.AzureMaps.Blazor.Models.Common;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers
{
    public class HtmlMarkerDef : JsInteropBase, ICloneable
    {
        public HtmlMarkerOptions Options { get; set { ArgumentNullException.ThrowIfNull(field, nameof(Options)); field = value; } } = new();

        /// <summary>
        /// If true, a click event will be added to toggle the popup.
        /// </summary>
        public bool TogglePopupOnClick { get; set; }

        /// <summary>
        /// <see cref="ICloneable"/>
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var clone = (HtmlMarkerDef)MemberwiseClone();
            clone.JsInterop = (JSInteropDef)JsInterop.Clone();
            clone.JsInterop = (JSInteropDef)JsInterop.Clone();
            clone.Options = (HtmlMarkerOptions)Options.Clone();

            return clone;
        }
    }

    public class HtmlMarkerOptions : ICloneable
    {
        /// <summary>
        /// Indicates the marker's location relative to its position on the map.
        /// Default "bottom"
        /// </summary>
        public PositionAnchor? Anchor { get; set; }

        /// <summary>
        /// A color value that replaces any {color} placeholder property that has been included in a string htmlContent.
        /// default "#1A73AA"
        /// </summary>
        public string? Color { get; set; }

        /// <summary>
        /// Indicates if the user can drag the position of the marker using the mouse or touch controls.
        /// default false
        /// </summary>
        public bool? Draggable { get; set; }

        /// <summary>
        /// The HTML content of the marker. Can be a string or an HTMLElement equivalent.
        /// Add {text} and {color} to HTML strings as placeholders to make it easy to update
        /// these values in your marker by using the setOptions function of the HtmlMarker class.
        /// This allows you to create a single HTML marker string that can be used as a template for multiple markers.
        /// </summary>
        public string? HtmlContent { get; set; }

        /// <summary>
        /// An offset in pixels to move the popup relative to the markers center.
        /// Negatives indicate left and up.
        /// default [0, 0]
        /// </summary>
        public Pixel? PixelOffset { get; set; }

        /// <summary>
        /// The position of the marker.
        /// default [0, 0]
        /// </summary>
        public Position? Position { get; set; }

        /// <summary>
        /// A popup that is attached to the marker.
        /// </summary>
        public PopupDef? Popup { get; set; }

        /// <summary>
        /// A color value that replaces any {secondaryColor} placeholder property that has been included in a string htmlContent.
        /// default "white"
        /// </summary>
        public string? SecondaryColor { get; set; }

        /// <summary>
        /// A string of text that replaces any {text} placeholder property that has been included in a string htmlContent.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Specifies if the marker is visible or not.
        /// default true
        /// </summary>
        public bool? Visible { get; set; }

        /// <summary>
        /// <see cref="ICloneable"/>
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var clone = (HtmlMarkerOptions)MemberwiseClone();
            clone.PixelOffset = PixelOffset?.Clone() as Pixel;
            clone.Position = Position?.Clone() as Position;
            clone.Popup = Popup?.Clone() as PopupDef;

            return clone;
        }
    }
}
