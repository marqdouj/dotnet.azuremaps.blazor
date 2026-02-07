using Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Input;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Services;
using Marqdouj.DotNet.Web.Components.FluentUI.UIInput;
using Marqdouj.DotNet.Web.Components.UI;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Layers
{
    public class BubbleLayerUIModel : LayerUIModel<BubbleLayerDef>, ICloneable
    {
        private readonly BubbleLayerOptionsUIModel options;

        public BubbleLayerUIModel(IAzureMapsUIXmlService? xmlService) : base(xmlService)
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

        public override BubbleLayerDef Source 
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
            items.RemoveAll(e => e.Name == nameof(BubbleLayerDef.Options));
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
            var clone = (BubbleLayerUIModel)MemberwiseClone();
            clone.Source = (BubbleLayerDef)Source.Clone();

            return clone;
        }

        public BubbleLayerOptionsUIModel Options => options;

        public override ILayerOptionsUIModel UIModelOptions => Options;
    }

    public class BubbleLayerOptionsUIModel : LayerSourceOptionsUIModel<BubbleLayerOptions>
    {
        private static readonly List<Option<string>> pitches = UIExtensions.GetEnumLookup<BubbleLayerPitchAlignment>(true);

        public BubbleLayerOptionsUIModel(IAzureMapsUIXmlService? xmlService) : base(xmlService)
        {
            Blur.SetBindMinMax(0, 1);
            Opacity.SetBindMinMax(0, 1);
            StrokeOpacity.SetBindMinMax(0, 1);
            StrokeWidth.SetBindMinMax(0, null);
            Radius.SetBindMinMax(0, null);
        }

        public override List<IUIModelInputValue> ToUIInputList()
        {
            var items = new List<IUIModelInputValue>
            {
                new UIModelInputValue(Color, UIModelInputType.Color),
                new UIModelInputValue(Blur, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(Opacity, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(StrokeColor, UIModelInputType.Color),
                new UIModelInputValue(StrokeOpacity, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(StrokeWidth, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(PitchAlignment, UIModelInputType.Select, lookup: pitches),
                new UIModelInputValue(Radius, UIModelInputType.Text, TextFieldType.Number)
            };

            items.AddRange(base.ToUIInputList());
            return items.SortUIModel();
        }

        public IUIModelValue Color => GetItem(nameof(BubbleLayerOptions.Color))!;
        public IUIModelValue Blur => GetItem(nameof(BubbleLayerOptions.Blur))!;
        public IUIModelValue Opacity => GetItem(nameof(BubbleLayerOptions.Opacity))!;
        public IUIModelValue StrokeColor => GetItem(nameof(BubbleLayerOptions.StrokeColor))!;
        public IUIModelValue StrokeOpacity => GetItem(nameof(BubbleLayerOptions.StrokeOpacity))!;
        public IUIModelValue StrokeWidth => GetItem(nameof(BubbleLayerOptions.StrokeWidth))!;
        public IUIModelValue PitchAlignment => GetItem(nameof(BubbleLayerOptions.PitchAlignment))!;
        public IUIModelValue Radius => GetItem(nameof(BubbleLayerOptions.Radius))!;
    }
}
