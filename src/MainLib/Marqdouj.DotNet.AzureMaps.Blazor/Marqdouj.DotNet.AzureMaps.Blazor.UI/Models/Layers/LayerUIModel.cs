using Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Input;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Services;
using Marqdouj.DotNet.Web.Components.UI;

namespace Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Layers
{
    public interface ILayerUIModel : IUIModel, IUIInputListSource, IUIModelReset
    {
        IUIModelValue Id { get; }
        IUIModelValue Before { get; }
        //IUIModelValue LayerType { get; }
        //IUIModelValue SourceId { get; }
        //IUIModelValue SourceUrl { get; }

        DataSourceUIModel DataSource { get; }

        MapLayerDef LayerDef { get; }

        ILayerOptionsUIModel UIModelOptions { get; }
    }

    public abstract class LayerUIModel<T> : XmlUIModel<T>, ILayerUIModel where T : MapLayerDef
    {
        private readonly DataSourceUIModel dataSource;

        protected internal LayerUIModel(IAzureMapsUIXmlService? xmlService) : base(xmlService)
        {
            dataSource = new(xmlService);

            //LayerType.SortOrder = -4;
            Id.SortOrder = -1;
            //dataSource.Id.SortOrder = -2;
            //dataSource.Url.SortOrder = -1;
        }

        public abstract ILayerOptionsUIModel UIModelOptions { get; }

        public MapLayerDef LayerDef => Source;

        public virtual new T Source 
        { 
            get => base.Source!; 
            set
            {
                base.Source = value;
                dataSource.Source = value.DataSource;
            }
        }

        //public abstract object GetOptions();

        public override List<IUIModelValue> ToUIList()
        {
            var items = base.ToUIList();
            items.RemoveAll(e => e.Name == nameof(MapLayerDef.DataSource));
            //items.AddRange(dataSource.ToUIList());

            return items.SortUIModel();
        }

        public virtual List<IUIModelInputValue> ToUIInputList()
        {
            var items = new List<IUIModelInputValue>
            {
                new UIModelInputValue(Id, UIModelInputType.Text) { Style=ModelExtensions.UIModelInputStyle_Id },
                new UIModelInputValue(Before, UIModelInputType.Text),
            };
            //items.AddRange(dataSource.ToUIInputList());

            return items.SortUIModel();
        }

        public abstract void ResetToDefaults(UIModelResetCategory category = UIModelResetCategory.Full);

        public DataSourceUIModel DataSource => dataSource;

        public IUIModelValue Before => GetItem(nameof(MapLayerDef.Before))!;
        public IUIModelValue Id => GetItem(nameof(MapLayerDef.Id))!;
        //public IUIModelValue LayerType => GetItem(nameof(MapLayerDef.LayerType))!;
        //public IUIModelValue SourceId => dataSource.Id!;
        //public IUIModelValue SourceUrl => dataSource.Url!;
    }
}
