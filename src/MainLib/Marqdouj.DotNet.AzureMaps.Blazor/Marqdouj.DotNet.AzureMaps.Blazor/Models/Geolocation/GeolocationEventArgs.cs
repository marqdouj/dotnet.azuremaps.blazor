using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Geolocation
{
    public enum GeolocationEventType
    {
        WatchSuccess,
        WatchError,
    }

    /// <summary>
    /// Provides data for geolocation-related events, including the event type, associated map identifier, and
    /// geolocation result.
    /// </summary>
    public class GeolocationEventArgs
    {
        /// <summary>
        /// <see cref="GeolocationEventType"/>
        /// </summary>
        [JsonIgnore]
        public GeolocationEventType? Type { get; internal set; }

        [JsonInclude]
        [JsonPropertyName("type")]
        internal string? TypeJs { get => Type.EnumToJsonN(); set => Type = value.JsonToEnumN<GeolocationEventType>(); }

        [JsonInclude]
        public string? MapId { get; internal set; }

        /// <summary>
        /// The <see cref="GeolocationResult"/> associated with the event.
        /// </summary>
        [JsonInclude]
        public GeolocationResult? Result { get; internal set; }

        [JsonIgnore]
        public bool IsSuccess => Result is not null && Result.IsSuccess;
    }
}
