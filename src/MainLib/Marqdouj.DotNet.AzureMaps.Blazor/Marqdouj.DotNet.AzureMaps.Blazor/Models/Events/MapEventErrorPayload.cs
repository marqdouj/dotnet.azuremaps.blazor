using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Events
{
    ///// <summary>
    ///// Returned when by the map when an error event occurs.
    ///// </summary>
    //public class MapEventErrorArgs : MapEventArgs<MapEventErrorPayload>
    //{
    //}

    /// <summary>
    /// MapEventError Payload.
    /// </summary>
    public class MapEventErrorPayload
    {
        /// <summary>
        /// Name
        /// </summary>
        [JsonInclude] public string? Name { get; internal set; }
        /// <summary>
        /// Message
        /// </summary>
        [JsonInclude] public string? Message { get; internal set; }
        /// <summary>
        /// Stack
        /// </summary>
        [JsonInclude] public string? Stack { get; internal set; }
        /// <summary>
        /// Cause (if known).
        /// </summary>
        [JsonInclude] public string? Cause { get; internal set; }

        public override string ToString()
        {
            return $"{Name} {Message}";
        }
    }
}
