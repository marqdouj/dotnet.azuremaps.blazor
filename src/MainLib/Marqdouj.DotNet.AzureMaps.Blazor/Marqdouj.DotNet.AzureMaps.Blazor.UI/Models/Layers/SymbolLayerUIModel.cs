using Marqdouj.DotNet.AzureMaps.Blazor.Models.Common;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Input;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Services;
using Marqdouj.DotNet.Web.Components.FluentUI.UIInput;
using Marqdouj.DotNet.Web.Components.UI;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Layers
{
    public class SymbolLayerUIModel : LayerUIModel<SymbolLayerDef>, ICloneable
    {
        private readonly SymbolLayerOptionsUIModel options;

        public SymbolLayerUIModel(IAzureMapsUIXmlService? xmlService) : base(xmlService)
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

        public override SymbolLayerDef Source
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
            items.RemoveAll(e => e.Name == nameof(SymbolLayerDef.Options));
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
            var clone = (SymbolLayerUIModel)MemberwiseClone();
            clone.Source = (SymbolLayerDef)Source.Clone();

            return clone;
        }

        public SymbolLayerOptionsUIModel Options => options;
        public override ILayerOptionsUIModel UIModelOptions => Options;
    }

    public class SymbolLayerOptionsUIModel : LayerSourceOptionsUIModel<SymbolLayerOptions>
    {
        private static readonly List<Option<string>> placements = UIExtensions.GetEnumLookup<SymbolLayerPlacement>(true);
        private static readonly List<Option<string>> zOrders = UIExtensions.GetEnumLookup<SymbolLayerZOrder>(true);

        public SymbolLayerOptionsUIModel(IAzureMapsUIXmlService? xmlService) : base(xmlService)
        {
            LineSpacing.SetBindMinMax(1, null);
        }

        public override List<IUIModelValue> ToUIList()
        {
            var items = base.ToUIList();
            items.RemoveAll(e => e.Name == nameof(SymbolLayerOptions.IconOptions) || e.Name == nameof(SymbolLayerOptions.TextOptions));
            return items.SortUIModel();
        }

        public override List<IUIModelInputValue> ToUIInputList()
        {
            var items = new List<IUIModelInputValue>
            {
                //new UIModelInputValue(IconOptions, UIModelInputType.IconOptions),
                new UIModelInputValue(LineSpacing, UIModelInputType.Text, TextFieldType.Number),
                //new UIModelInputValue(TextOptions, UIModelInputType.TextOptions),
                new UIModelInputValue(Placement, UIModelInputType.Select, lookup: placements),
                new UIModelInputValue(SortKey, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(ZOrder, UIModelInputType.Select, lookup: zOrders),
            };

            items.AddRange(base.ToUIInputList());
            return items.SortUIModel();
        }

        public IUIModelValue IconOptions => GetItem(nameof(SymbolLayerOptions.IconOptions))!;
        public IUIModelValue LineSpacing => GetItem(nameof(SymbolLayerOptions.LineSpacing))!;
        public IUIModelValue TextOptions => GetItem(nameof(SymbolLayerOptions.TextOptions))!;
        public IUIModelValue Placement => GetItem(nameof(SymbolLayerOptions.Placement))!;
        public IUIModelValue SortKey => GetItem(nameof(SymbolLayerOptions.SortKey))!;
        public IUIModelValue ZOrder => GetItem(nameof(SymbolLayerOptions.ZOrder))!;
    }

    public class IconOptionsUIModel : XmlUIModel<IconOptions>
    {
        private static readonly List<Option<string>> booleans = UILookups.GetBooleans(true);
        private static readonly List<Option<string>> anchors = UIExtensions.GetEnumLookup<PositionAnchor>(true);
        private static readonly List<Option<string>> icons = UIExtensions.GetEnumLookup<IconImage>(true);
        private static readonly List<Option<string>> alignments = UIExtensions.GetEnumLookup<MapItemAlignment>(true);

        public IconOptionsUIModel(IAzureMapsUIXmlService? xmlService) : base(xmlService)
        {
            Opacity.SetBindMinMax(0, 1);
            Rotation.SetBindMinMax(0, 360);
            Size.SetBindMinMax(0, null);
        }

        public List<IUIModelInputValue> ToUIInputList()
        {
            var items = new List<IUIModelInputValue>
            {
                new UIModelInputValue(AllowOverlap, UIModelInputType.Select, lookup: booleans),
                new UIModelInputValue(Anchor, UIModelInputType.Select, lookup: anchors),
                new UIModelInputValue(IgnorePlacement, UIModelInputType.Select, lookup: booleans),
                new UIModelInputValue(Image, UIModelInputType.Select, lookup: icons),
                new UIModelInputValue(Offset, UIModelInputType.Pixel),
                new UIModelInputValue(Opacity, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(Optional, UIModelInputType.Select, lookup: booleans),
                new UIModelInputValue(Padding, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(PitchAlignment, UIModelInputType.Select, lookup: alignments),
                new UIModelInputValue(Rotation, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(RotationAlignment, UIModelInputType.Select, lookup: alignments),
                new UIModelInputValue(Size, UIModelInputType.Text, TextFieldType.Number),
            };

            return items.SortUIModel();
        }

        public IUIModelValue AllowOverlap => GetItem(nameof(IconOptions.AllowOverlap))!;
        public IUIModelValue Anchor => GetItem(nameof(IconOptions.Anchor))!;
        public IUIModelValue IgnorePlacement => GetItem(nameof(IconOptions.IgnorePlacement))!;
        public IUIModelValue Image => GetItem(nameof(IconOptions.Image))!;
        public IUIModelValue Offset => GetItem(nameof(IconOptions.Offset))!;
        public IUIModelValue Opacity => GetItem(nameof(IconOptions.Opacity))!;
        public IUIModelValue Optional => GetItem(nameof(IconOptions.Optional))!;
        public IUIModelValue Padding => GetItem(nameof(IconOptions.Padding))!;
        public IUIModelValue PitchAlignment => GetItem(nameof(IconOptions.PitchAlignment))!;
        public IUIModelValue Rotation => GetItem(nameof(IconOptions.Rotation))!;
        public IUIModelValue RotationAlignment => GetItem(nameof(IconOptions.RotationAlignment))!;
        public IUIModelValue Size => GetItem(nameof(IconOptions.Size))!;
    }

    public class TextOptionsUIModel : XmlUIModel<TextOptions>
    {
        private static readonly List<Option<string>> booleans = UILookups.GetBooleans(true);
        private static readonly List<Option<string>> anchors = UIExtensions.GetEnumLookup<PositionAnchor>(true);
        private static readonly List<Option<string>> justifies = UIExtensions.GetEnumLookup<TextJustify>(true);
        private static readonly List<Option<string>> alignments = UIExtensions.GetEnumLookup<MapItemAlignment>(true);

        public TextOptionsUIModel(IAzureMapsUIXmlService? xmlService) : base(xmlService)
        {
            HaloBlur.SetBindMinMax(0, null);
            HaloWidth.SetBindMinMax(0, null);
            Opacity.SetBindMinMax(0, 1);
            Padding.SetBindMinMax(0, null);
            Rotation.SetBindMinMax(0, 360);
            Size.SetBindMinMax(0, null);
        }

        public List<IUIModelInputValue> ToUIInputList()
        {
            var items = new List<IUIModelInputValue>
            {
                new UIModelInputValue(AllowOverlap, UIModelInputType.Select, lookup: booleans),
                new UIModelInputValue(Anchor, UIModelInputType.Select, lookup: anchors),
                new UIModelInputValue(Color, UIModelInputType.Color),
                //new UIModelInputValue(Font, UIModelInputType.Text, TextFieldType.Text),
                new UIModelInputValue(HaloBlur, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(HaloColor, UIModelInputType.Color),
                new UIModelInputValue(HaloWidth, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(IgnorePlacement, UIModelInputType.Select, lookup: booleans),
                new UIModelInputValue(Justify, UIModelInputType.Select, lookup: justifies),
                new UIModelInputValue(Offset, UIModelInputType.Pixel),
                new UIModelInputValue(Opacity, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(Optional, UIModelInputType.Select, lookup: booleans),
                new UIModelInputValue(Padding, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(PitchAlignment, UIModelInputType.Select, lookup: alignments),
                new UIModelInputValue(RadialOffset, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(Rotation, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(RotationAlignment, UIModelInputType.Select, lookup: alignments),
                new UIModelInputValue(Size, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(TextField, UIModelInputType.Text),
                new UIModelInputValue(VariableAnchor, UIModelInputType.Select, lookup: anchors),
            };

            return items.SortUIModel();
        }

        public IUIModelValue AllowOverlap => GetItem(nameof(TextOptions.AllowOverlap))!;
        public IUIModelValue Anchor => GetItem(nameof(TextOptions.Anchor))!;
        public IUIModelValue Color => GetItem(nameof(TextOptions.Color))!;
        //public IUIModelValue Font => GetItem(nameof(TextOptions.Font))!;
        public IUIModelValue HaloBlur => GetItem(nameof(TextOptions.HaloBlur))!;
        public IUIModelValue HaloColor => GetItem(nameof(TextOptions.HaloColor))!;
        public IUIModelValue HaloWidth => GetItem(nameof(TextOptions.HaloWidth))!;
        public IUIModelValue IgnorePlacement => GetItem(nameof(TextOptions.IgnorePlacement))!;
        public IUIModelValue Justify => GetItem(nameof(TextOptions.Justify))!;
        public IUIModelValue Offset => GetItem(nameof(TextOptions.Offset))!;
        public IUIModelValue Opacity => GetItem(nameof(TextOptions.Opacity))!;
        public IUIModelValue Optional => GetItem(nameof(TextOptions.Optional))!;
        public IUIModelValue Padding => GetItem(nameof(TextOptions.Padding))!;
        public IUIModelValue PitchAlignment => GetItem(nameof(TextOptions.PitchAlignment))!;
        public IUIModelValue RadialOffset => GetItem(nameof(TextOptions.RadialOffset))!;
        public IUIModelValue Rotation => GetItem(nameof(TextOptions.Rotation))!;
        public IUIModelValue RotationAlignment => GetItem(nameof(TextOptions.RotationAlignment))!;
        public IUIModelValue Size => GetItem(nameof(TextOptions.Size))!;
        public IUIModelValue TextField => GetItem(nameof(TextOptions.TextField))!;
        public IUIModelValue VariableAnchor => GetItem(nameof(TextOptions.VariableAnchor))!;
    }
}
