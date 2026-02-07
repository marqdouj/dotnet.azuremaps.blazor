using Marqdouj.DotNet.AzureMaps.Blazor.Models.Common;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Input;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Services;
using Marqdouj.DotNet.Web.Components.UI;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Common
{
    public class PixelUIModel(IAzureMapsUIXmlService? xmlService) : XmlUIModel<Pixel>(xmlService)
    {
        public List<IUIModelInputValue> ToUIInputList()
        {
            var items = new List<IUIModelInputValue>
            {
                new UIModelInputValue(X, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(Y, UIModelInputType.Text, TextFieldType.Number),
            };

            return items;
        }

        public IUIModelValue X => GetItem(nameof(Pixel.X))!;
        public IUIModelValue Y => GetItem(nameof(Pixel.Y))!;
    }
}
