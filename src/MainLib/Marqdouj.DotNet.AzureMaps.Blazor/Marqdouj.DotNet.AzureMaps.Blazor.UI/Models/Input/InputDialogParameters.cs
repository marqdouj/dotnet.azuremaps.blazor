namespace Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Input
{
    public class InputDialogParameters(IUIModelReset model, List<IUIModelInputValue> inputs, UIModelResetCategory? resetCategory = null)
    {
        public string? Height { get; set; }
        public List<IUIModelInputValue> Inputs { get; } = inputs;
        public IUIModelReset Model { get; } = model;
        public UIModelResetCategory? ResetCategory { get; } = resetCategory;
        public string? Style { get; set; }
        public string? Width { get; set; }
    }
}
