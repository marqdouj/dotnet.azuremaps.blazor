using Marqdouj.DotNet.AzureMaps.Blazor.Models.Common;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers
{
    public class PopupDef : JsInteropBase, ICloneable
    {
        public PopupOptions Options { get; set { ArgumentNullException.ThrowIfNull(field, nameof(Options)); field = value; } } = new();

        /// <summary>
        /// <see cref="ICloneable"/>
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var clone = (PopupDef)MemberwiseClone();
            clone.JsInterop = (JSInteropDef)JsInterop.Clone();
            clone.Options = (PopupOptions)Options.Clone();

            return clone;
        }
    }

    public class PopupOptions : ICloneable
    {
        /// <summary>
        /// Specifies if the popup can be dragged away from its position.
        /// default false
        /// </summary>
        public bool? Draggable { get; set; }

        /// <summary>
        /// Specifies if the close button should be displayed in the popup or not.
        /// default true
        /// </summary>
        public bool? CloseButton { get; set; }

        /// <summary>
        /// The content to display within the popup.
        /// default span
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// Specifies the fill color of the popup.
        /// default "#FFFFFF"
        /// </summary>
        public string? FillColor { get; set; }

        /// <summary>
        /// How many pixels to the right and down the anchor of the popup should be offset.
        /// Negative numbers can be used to offset the popup left and up.
        /// default [0, 0]
        /// </summary>
        public Pixel? PixelOffset { get; set; }

        /// <summary>
        /// The position on the map where the popup should be anchored.
        /// default [0, 0]
        /// </summary>
        public Position? Position { get; set; }

        /// <summary>
        /// Specifies if the pointer should be displayed in the popup or not.
        /// default true
        /// </summary>
        public bool? ShowPointer { get; set; }

        /// <summary>
        /// <see cref="ICloneable"/>
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var clone = (PopupOptions)MemberwiseClone();
            clone.PixelOffset = PixelOffset?.Clone() as Pixel;
            clone.Position = Position?.Clone() as Position;

            return clone;
        }
    }
}
