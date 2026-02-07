using Marqdouj.DotNet.AzureMaps.Blazor.Models.Configuration;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Input;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Services;
using Marqdouj.DotNet.Web.Components.FluentUI.UIInput;
using Marqdouj.DotNet.Web.Components.UI;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Configuration
{
    public class TrafficOptionsUIModel(IAzureMapsUIXmlService? xmlService) : XmlUIModel<TrafficOptions>(xmlService), IUIInputListSource
    {
        private static readonly List<Option<string>> flows = UIExtensions.GetEnumLookup<TrafficFlow>(true);

        public virtual List<IUIModelInputValue> ToUIInputList()
        {
            var items = new List<IUIModelInputValue>
            {
                new UIModelInputValue(Flow, UIModelInputType.Select, lookup: flows),
                new UIModelInputValue(Incidents, UIModelInputType.Select, lookup: UILookups.GetBooleans(true)),
            };

            return items;
        }

        public IUIModelValue Flow => GetItem(nameof(TrafficOptions.Flow))!;
        public IUIModelValue Incidents => GetItem(nameof(TrafficOptions.Incidents))!;
    }
}
