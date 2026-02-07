namespace Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Input
{
    public enum UIModelResetCategory
    {
        Full,
        Options,
    }

    public interface IUIModelReset
    {
        void ResetToDefaults(UIModelResetCategory category = UIModelResetCategory.Full);
    }
}
