using Marqdouj.DotNet.AzureMaps.Blazor.UI.Components;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Input;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Marqdouj.DotNet.AzureMaps.Blazor.UI.Models
{
    public static class DialogExtentions
    {
        public static DialogParameters GetDefaultDialogParameters(string title, string? width = "80%")
        {
            DialogParameters parameters = new()
            {
                Title = title,
                PrimaryAction = "OK",
                PrimaryActionEnabled = true,
                SecondaryAction = "Cancel",
                Width = width,
                TrapFocus = true,
                Modal = false,
                PreventScroll = true
            };

            return parameters;
        }

        public static async Task<DialogResult> ShowInputsDialog(this IDialogService service, string title, InputDialogParameters inputParameters, DialogParameters? dialogParameters = null)
        {
            dialogParameters ??= GetDefaultDialogParameters(title);
            IDialogReference dialog = await service.ShowDialogAsync<UIModelInputDialog>(inputParameters, dialogParameters);
            var result = await dialog.Result;

            return result;
        }

        public static async Task<DialogResult> ShowLayerInputsDialog(this IDialogService service, string title, InputLayersDialogParameters input, DialogParameters? parameters = null)
        {
            parameters ??= GetDefaultDialogParameters(title);
            IDialogReference dialog = await service.ShowDialogAsync<UIModelInputLayersDialog>(input, parameters);
            var result = await dialog.Result;

            return result;
        }
    }
}
