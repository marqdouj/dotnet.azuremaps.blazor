using Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Input;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Services;
using Marqdouj.DotNet.Web.Components.FluentUI.UIInput;
using Marqdouj.DotNet.Web.Components.UI;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Layers
{
    public class LineLayerUIModel : LayerUIModel<LineLayerDef>, ICloneable
    {
        private readonly LineLayerOptionsUIModel options;

        public LineLayerUIModel(IAzureMapsUIXmlService? xmlService) : base(xmlService)
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

        public override LineLayerDef Source
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
            items.RemoveAll(e => e.Name == nameof(LineLayerDef.Options));
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
            var clone = (LineLayerUIModel)MemberwiseClone();
            clone.Source = (LineLayerDef)Source.Clone();

            return clone;
        }

        public LineLayerOptionsUIModel Options => options;
        public override ILayerOptionsUIModel UIModelOptions => Options;
    }

    public class LineLayerOptionsUIModel : LayerSourceOptionsUIModel<LineLayerOptions>
    {
        private static readonly List<Option<string>> lineCaps = UIExtensions.GetEnumLookup<LineCap>(true);
        private static readonly List<Option<string>> lineJoins = UIExtensions.GetEnumLookup<LineJoin>(true);
        private static readonly List<Option<string>> anchors = UIExtensions.GetEnumLookup<TranslateAnchor>(true);

        public LineLayerOptionsUIModel(IAzureMapsUIXmlService? xmlService) : base(xmlService)
        {
            Blur.SetBindMinMax(0, 1);
            StrokeOpacity.SetBindMinMax(0, 1);
            StrokeWidth.SetBindMinMax(0, null);
        }

        public override List<IUIModelInputValue> ToUIInputList()
        {
            var items = new List<IUIModelInputValue>
            {
                new UIModelInputValue(Blur, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(LineCap, UIModelInputType.Select, lookup: lineCaps),
                new UIModelInputValue(LineJoin, UIModelInputType.Select, lookup: lineJoins),
                new UIModelInputValue(Offset, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(StrokeColor, UIModelInputType.Color),
                new UIModelInputValue(StrokeOpacity, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(StrokeWidth, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(Translate, UIModelInputType.Pixel),
                new UIModelInputValue(TranslateAnchor, UIModelInputType.Select, lookup: anchors),
            };

            items.AddRange(base.ToUIInputList());
            return items.SortUIModel();
        }

        public IUIModelValue Blur => GetItem(nameof(LineLayerOptions.Blur))!;
        public IUIModelValue LineCap => GetItem(nameof(LineLayerOptions.LineCap))!;
        public IUIModelValue LineJoin => GetItem(nameof(LineLayerOptions.LineJoin))!;
        public IUIModelValue Offset => GetItem(nameof(LineLayerOptions.Offset))!;
        public IUIModelValue StrokeColor => GetItem(nameof(LineLayerOptions.StrokeColor))!;
        public IUIModelValue StrokeOpacity => GetItem(nameof(LineLayerOptions.StrokeOpacity))!;
        public IUIModelValue StrokeWidth => GetItem(nameof(LineLayerOptions.StrokeWidth))!;
        public IUIModelValue Translate => GetItem(nameof(LineLayerOptions.Translate))!;
        public IUIModelValue TranslateAnchor => GetItem(nameof(LineLayerOptions.TranslateAnchor))!;
    }
}
