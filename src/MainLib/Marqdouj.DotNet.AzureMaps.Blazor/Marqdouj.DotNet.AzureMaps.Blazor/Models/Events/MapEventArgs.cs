using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Events
{
    /// <summary>
    /// Base class for all map events.
    /// </summary>
    public class MapEventArgs: MapEventBase
    {
        /// <summary>
        /// Map container (div) id.
        /// </summary>
        [JsonInclude] public string? MapId { get; internal set; }

        /// <summary>
        /// <see cref="System.Object.ToString"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{MapId}-{Type}";

        public MapEventArgsPayload? Payload { get; set; }
    }
}
