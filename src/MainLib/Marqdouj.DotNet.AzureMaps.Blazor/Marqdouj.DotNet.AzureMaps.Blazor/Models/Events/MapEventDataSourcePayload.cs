using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Events
{
    public class MapEventDataSourcePayload
    {
        public string? Id { get; set; }
        /// <summary>
        /// An array of Shape and Feature objects associated with the event.
        /// </summary>
        [JsonInclude] public List<MapEventShape>? Shapes { get; internal set; }
    }
}
