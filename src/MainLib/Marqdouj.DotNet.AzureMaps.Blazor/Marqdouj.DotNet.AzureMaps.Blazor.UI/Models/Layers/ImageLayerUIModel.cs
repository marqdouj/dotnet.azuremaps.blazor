using Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Input;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Services;
using Marqdouj.DotNet.Web.Components.UI;

namespace Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Layers
{
    public class ImageLayerUIModel : LayerUIModel<ImageLayerDef>, ICloneable
    {
        private readonly ImageLayerOptionsUIModel options;

        public ImageLayerUIModel(IAzureMapsUIXmlService? xmlService) : base(xmlService)
        {
            options = new(xmlService);
            Source = new();

            options.Url.SortOrder = -1;
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

        public override ImageLayerDef Source
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
            items.RemoveAll(e => e.Name == nameof(ImageLayerDef.Options));
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
            var clone = (ImageLayerUIModel)MemberwiseClone();
            clone.Source = (ImageLayerDef)Source.Clone();

            return clone;
        }

        public ImageLayerOptionsUIModel Options => options;
        public override ILayerOptionsUIModel UIModelOptions => Options;
    }

    public class ImageLayerOptionsUIModel : LayerMediaOptionsUIModel<ImageLayerOptions>
    {
        public ImageLayerOptionsUIModel(IAzureMapsUIXmlService? xmlService) : base(xmlService)
        {
            Url?.NameAlias = "Image URL";
            Url?.ReadOnly = true;
        }

        public override List<IUIModelInputValue> ToUIInputList()
        {
            var items = new List<IUIModelInputValue>
            {
                //new UIModelInputValue(Coordinates, UIModelInputType.ImageCoordinates),
                new UIModelInputValue(Url, UIModelInputType.Text) { Style=ModelExtensions.UIModelInputStyle_Url },
            };

            items.AddRange(base.ToUIInputList());
            return items.SortUIModel();
        }

        public IUIModelValue Coordinates => GetItem(nameof(ImageLayerOptions.Coordinates))!;
        public IUIModelValue Url => GetItem(nameof(ImageLayerOptions.Url))!;
    }
}
