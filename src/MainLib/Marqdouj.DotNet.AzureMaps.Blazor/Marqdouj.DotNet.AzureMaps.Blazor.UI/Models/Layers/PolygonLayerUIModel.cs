using Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Input;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Services;
using Marqdouj.DotNet.Web.Components.UI;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Layers
{
    public class PolygonLayerUIModel : LayerUIModel<PolygonLayerDef>, ICloneable
    {
        private readonly PolygonLayerOptionsUIModel options;

        public PolygonLayerUIModel(IAzureMapsUIXmlService? xmlService) : base(xmlService)
        {
            options = new(xmlService);
            Source = new();
        }

        public override void ResetToDefaults(UIModelResetCategory category = UIModelResetCategory.Full)
        {
            switch (category)
            {
                case UIModelResetCategory.Full:
                    Source = new();
                    break;
                case UIModelResetCategory.Options:
                    Source.Options = new();
                    options.Source = Source.Options;
                    break;
                default:
                    break;
            }
        }

        public override PolygonLayerDef Source
        {
            get => base.Source;
            set
            {
                value ??= new();
                base.Source = value;
                options.Source = value.Options;
            }
        }

        public override List<IUIModelValue> ToUIList()
        {
            var items = base.ToUIList();
            items.RemoveAll(e => e.Name == nameof(PolygonLayerDef.Options));
            items.AddRange(options.ToUIList());

            return items.SortUIModel();
        }

        public override List<IUIModelInputValue> ToUIInputList()
        {
            var items = base.ToUIInputList();
            items.AddRange(options.ToUIInputList());

            return items.SortUIModel();
        }

        /// <summary>
        /// <see cref="ICloneable"/>
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var clone = (PolygonLayerUIModel)MemberwiseClone();
            clone.Source = (PolygonLayerDef)Source.Clone();

            return clone;
        }

        public PolygonLayerOptionsUIModel Options => options;
        public override ILayerOptionsUIModel UIModelOptions => Options;
    }

    public class PolygonLayerOptionsUIModel : LayerSourceOptionsUIModel<PolygonLayerOptions>
    {
        public PolygonLayerOptionsUIModel(IAzureMapsUIXmlService? xmlService) : base(xmlService)
        {
            FillOpacity.SetBindMinMax(0, 1);
        }

        public override List<IUIModelInputValue> ToUIInputList()
        {
            var items = new List<IUIModelInputValue>
            {
                new UIModelInputValue(FillAntialias, UIModelInputType.Select, lookup: UILookups.GetBooleans(true)),
                new UIModelInputValue(FillColor, UIModelInputType.Color),
                new UIModelInputValue(FillOpacity, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(FillPattern, UIModelInputType.Text, TextFieldType.Text),
            };

            items.AddRange(base.ToUIInputList());
            return items.SortUIModel();
        }

        public IUIModelValue FillAntialias => GetItem(nameof(PolygonLayerOptions.FillAntialias))!;
        public IUIModelValue FillColor => GetItem(nameof(PolygonLayerOptions.FillColor))!;
        public IUIModelValue FillOpacity => GetItem(nameof(PolygonLayerOptions.FillOpacity))!;
        public IUIModelValue FillPattern => GetItem(nameof(PolygonLayerOptions.FillPattern))!;
    }
}
