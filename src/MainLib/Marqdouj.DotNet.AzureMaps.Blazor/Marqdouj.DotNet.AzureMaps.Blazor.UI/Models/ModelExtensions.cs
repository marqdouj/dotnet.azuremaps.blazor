using Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Input;
using Marqdouj.DotNet.Web.Components.UI;

namespace Marqdouj.DotNet.AzureMaps.Blazor.UI.Models
{
    internal static class ModelExtensions
    {
        public static readonly string UIModelInputStyle_Id = "width:300px;"; 
        public static readonly string UIModelInputStyle_Url = "width:300px;";

        public static List<IUIModelValue> SortUIModel(this List<IUIModelValue> values)
        {
            return [.. values.OrderBy(e => e.SortOrder).ThenBy(e => e.NameDisplay)];
        }

        public static List<IUIModelInputValue> SortUIModel(this List<IUIModelInputValue> values)
        {
            return [.. values.OrderBy(e => e.SortOrder).ThenBy(e => e.NameDisplay)];
        }
    }
}
