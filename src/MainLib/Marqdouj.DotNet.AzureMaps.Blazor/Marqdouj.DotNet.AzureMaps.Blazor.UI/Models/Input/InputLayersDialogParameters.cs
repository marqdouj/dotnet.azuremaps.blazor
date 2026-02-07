using Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Layers;

namespace Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Input
{
    public class InputLayersDialogParameters(ILayerUIModel model, UIModelResetCategory? resetCategory = UIModelResetCategory.Full)
    {
        public string? Height { get; set; }
        public ILayerUIModel Model { get; } = model;
        public UIModelResetCategory? ResetCategory { get; } = resetCategory;
        public string? Style { get; set; }
        public string? Width { get; set; }
    }
}
