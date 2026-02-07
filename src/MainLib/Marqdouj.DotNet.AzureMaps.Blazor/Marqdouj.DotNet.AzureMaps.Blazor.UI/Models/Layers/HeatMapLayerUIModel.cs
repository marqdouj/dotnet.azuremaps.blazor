using Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Input;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Services;
using Marqdouj.DotNet.Web.Components.UI;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Layers
{
    public class HeatMapLayerUIModel : LayerUIModel<HeatMapLayerDef>, ICloneable
    {
        private readonly HeatMapLayerOptionsUIModel options;

        public HeatMapLayerUIModel(IAzureMapsUIXmlService? xmlService) : base(xmlService)
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

        public override HeatMapLayerDef Source
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
            items.RemoveAll(e => e.Name == nameof(HeatMapLayerDef.Options));
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
            var clone = (HeatMapLayerUIModel)MemberwiseClone();
            clone.Source = (HeatMapLayerDef)Source.Clone();

            return clone;
        }

        public HeatMapLayerOptionsUIModel Options => options;
        public override ILayerOptionsUIModel UIModelOptions => Options;
    }

    public class HeatMapLayerOptionsUIModel : LayerSourceOptionsUIModel<HeatMapLayerOptions>
    {
        public HeatMapLayerOptionsUIModel(IAzureMapsUIXmlService? xmlService) : base(xmlService)
        {
            Opacity.SetBindMinMax(0, 1);
            Radius.SetBindMinMax(1, null);
            Weight.SetBindMinMax(0, null);
        }

        public override List<IUIModelInputValue> ToUIInputList()
        {
            var items = new List<IUIModelInputValue>
            {
                new UIModelInputValue(Intensity, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(Opacity, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(Radius, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(Weight, UIModelInputType.Text, TextFieldType.Number),
            };

            items.AddRange(base.ToUIInputList());
            return items.SortUIModel();
        }


        public IUIModelValue Intensity => GetItem(nameof(HeatMapLayerOptions.Intensity))!;
        public IUIModelValue Opacity => GetItem(nameof(HeatMapLayerOptions.Opacity))!;
        public IUIModelValue Radius => GetItem(nameof(HeatMapLayerOptions.Radius))!;
        public IUIModelValue Weight => GetItem(nameof(HeatMapLayerOptions.Weight))!;
    }
}
