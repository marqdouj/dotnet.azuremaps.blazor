using Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Input;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Services;
using Marqdouj.DotNet.Web.Components.UI;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Layers
{
    public class TileLayerUIModel : LayerUIModel<TileLayerDef>, ICloneable
    {
        private readonly TileLayerOptionsUIModel options;

        public TileLayerUIModel(IAzureMapsUIXmlService? xmlService) : base(xmlService)
        {
            options = new(xmlService);
            Source = new();

            options.TileUrl.SortOrder = -1;
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

        public override TileLayerDef Source
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
            items.RemoveAll(e => e.Name == nameof(TileLayerDef.Options));
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
            var clone = (TileLayerUIModel)MemberwiseClone();
            clone.Source = (TileLayerDef)Source.Clone();

            return clone;
        }

        public TileLayerOptionsUIModel Options => options;
        public override ILayerOptionsUIModel UIModelOptions => Options;
    }

    public class TileLayerOptionsUIModel : LayerMediaOptionsUIModel<TileLayerOptions>
    {
        private static readonly List<Option<string>> booleans = UILookups.GetBooleans(true);

        public TileLayerOptionsUIModel(IAzureMapsUIXmlService? xmlService) : base(xmlService)
        {
            MinSourceZoom.SetBindMinMax(0, 24);
            MaxSourceZoom.SetBindMinMax(0, 24);
        }

        public override List<IUIModelInputValue> ToUIInputList()
        {
            var items = new List<IUIModelInputValue>
            {
                //new UIModelInputValue(Bounds, UIModelInputType.BoundingBox),
                new UIModelInputValue(IsTMS, UIModelInputType.Select, lookup: booleans),
                new UIModelInputValue(MinSourceZoom, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(MaxSourceZoom, UIModelInputType.Text, TextFieldType.Number),
                new UIModelInputValue(TileSize, UIModelInputType.Text, TextFieldType.Number),
                //new UIModelInputValue(Subdomains, UIModelInputType.Text),
                new UIModelInputValue(TileUrl, UIModelInputType.Text) { Style=ModelExtensions.UIModelInputStyle_Url },
            };

            items.AddRange(base.ToUIInputList());
            return items.SortUIModel();
        }

        public IUIModelValue Bounds => GetItem(nameof(TileLayerOptions.Bounds))!;
        public IUIModelValue IsTMS => GetItem(nameof(TileLayerOptions.IsTMS))!;
        public IUIModelValue MinSourceZoom => GetItem(nameof(TileLayerOptions.MinSourceZoom))!;
        public IUIModelValue MaxSourceZoom => GetItem(nameof(TileLayerOptions.MaxSourceZoom))!;
        public IUIModelValue TileSize => GetItem(nameof(TileLayerOptions.TileSize))!;
        //public IUIModelValue Subdomains => GetItem(nameof(TileLayerOptions.Subdomains))!;
        public IUIModelValue TileUrl => GetItem(nameof(TileLayerOptions.TileUrl))!;
    }
}
