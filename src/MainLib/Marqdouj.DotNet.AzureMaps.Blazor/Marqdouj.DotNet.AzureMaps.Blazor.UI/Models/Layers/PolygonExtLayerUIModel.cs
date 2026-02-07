using Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Input;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Services;
using Marqdouj.DotNet.Web.Components.FluentUI.UIInput;
using Marqdouj.DotNet.Web.Components.UI;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Layers
{
    public class PolygonExtLayerUIModel : LayerUIModel<PolygonExtLayerDef>, ICloneable
    {
        private readonly PolygonExtLayerOptionsUIModel options;

        public PolygonExtLayerUIModel(IAzureMapsUIXmlService? xmlService) : base(xmlService)
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

        public override PolygonExtLayerDef Source
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
            items.RemoveAll(e => e.Name == nameof(PolygonExtLayerDef.Options));
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
            var clone = (PolygonExtLayerUIModel)MemberwiseClone();
            clone.Source = (PolygonExtLayerDef)Source.Clone();

            return clone;
        }

        public PolygonExtLayerOptionsUIModel Options => options;
        public override ILayerOptionsUIModel UIModelOptions => Options;
    }

    public class PolygonExtLayerOptionsUIModel : LayerSourceOptionsUIModel<PolygonExtLayerOptions>
    {
        private static readonly List<Option<string>> anchors = UIExtensions.GetEnumLookup<TranslateAnchor>(true);

        public PolygonExtLayerOptionsUIModel(IAzureMapsUIXmlService? xmlService) : base(xmlService)
        {
            Base.SetBindMinMax(0, null);
            FillOpacity.SetBindMinMax(0, 1);
            Height.SetBindMinMax(0, null);
        }

        public override List<IUIModelInputValue> ToUIInputList()
        {
            var items = new List<IUIModelInputValue>
            {
                new UIModelInputValue(Base, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(FillColor, UIModelInputType.Color),
                new UIModelInputValue(FillOpacity, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(FillPattern, UIModelInputType.Text, TextFieldType.Text),
                new UIModelInputValue(Height, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(Translate, UIModelInputType.Pixel),
                new UIModelInputValue(TranslateAnchor, UIModelInputType.Select, lookup: anchors),
                new UIModelInputValue(VerticalGradient, UIModelInputType.Select, lookup: anchors),
            };

            items.AddRange(base.ToUIInputList());
            return items.SortUIModel();
        }

        public IUIModelValue Base => GetItem(nameof(PolygonExtLayerOptions.Base))!;
        public IUIModelValue FillColor => GetItem(nameof(PolygonExtLayerOptions.FillColor))!;
        public IUIModelValue FillOpacity => GetItem(nameof(PolygonExtLayerOptions.FillOpacity))!;
        public IUIModelValue FillPattern => GetItem(nameof(PolygonExtLayerOptions.FillPattern))!;
        public IUIModelValue Height => GetItem(nameof(PolygonExtLayerOptions.Height))!;
        public IUIModelValue Translate => GetItem(nameof(PolygonExtLayerOptions.Translate))!;
        public IUIModelValue TranslateAnchor => GetItem(nameof(PolygonExtLayerOptions.TranslateAnchor))!;
        public IUIModelValue VerticalGradient => GetItem(nameof(PolygonExtLayerOptions.VerticalGradient))!;
    }
}
