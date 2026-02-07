using Marqdouj.DotNet.AzureMaps.Blazor.Models.Events;

namespace Sandbox.Components.Pages.AzureMaps.MpsCntnr.Events
{
    internal class MapEventViewModel
    {
        public MapEventViewModel(MapEventDef eventDef)
        {
            EventDef = eventDef;
            EventDef.PreventDefault = true;
        }

        public MapEventDef EventDef { get; }
        public string? Name => EventDef.Type.ToString();
        public MapEventType? Type => EventDef.Type;
        public bool IsChecked { get; set; }
        public bool IsNotChecked => !IsChecked;
        public bool IsLoaded { get; set; }
        public bool IsNotLoaded => !IsLoaded;


    }
}
