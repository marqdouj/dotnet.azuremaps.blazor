using Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Input;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Services;
using Marqdouj.DotNet.Web.Components.UI;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Layers
{
    public class ImageCoordinatesUIModel(IAzureMapsUIXmlService? xmlService) : XmlUIModel<ImageCoordinates>(xmlService)
    {
        internal List<IUIModelInputValue> ToUIInputList()
        {
            var items = new List<IUIModelInputValue>
            {
                new UIModelInputValue(TopLeft, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(TopRight, UIModelInputType.Text, TextFieldType.Number),                
                new UIModelInputValue(BottomLeft, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(BottomRight, UIModelInputType.Text, TextFieldType.Number),
            };

            return items.SortUIModel();
        }

        public IUIModelValue TopLeft => GetItem(nameof(ImageCoordinates.TopLeft))!;
        public IUIModelValue TopRight => GetItem(nameof(ImageCoordinates.TopRight))!;
        public IUIModelValue BottomLeft => GetItem(nameof(ImageCoordinates.BottomLeft))!;
        public IUIModelValue BottomRight => GetItem(nameof(ImageCoordinates.BottomRight))!;
    }
}
