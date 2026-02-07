using Marqdouj.DotNet.AzureMaps.Blazor.Models.Common;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Events
{
    public class MapEventTouchPayload
    {
        /// <summary>
        /// The pixel coordinate at the center of all touch points on the map, relative to the top left corner.
        /// </summary>
        [JsonInclude] public Pixel? Pixel { get; internal set; }

        /// <summary>
        /// The array of pixel coordinates of all touch points on the map.
        /// </summary>
        [JsonInclude] public List<Pixel>? Pixels { get; internal set; }

        /// <summary>
        /// The geographic location of the center of all touch points on the map.
        /// </summary>
        [JsonInclude] public Position? Position { get; internal set; }

        /// <summary>
        /// The geographical location of all touch points on the map.
        /// </summary>
        [JsonInclude] public List<Position>? Positions { get; internal set; }

        /// <summary>
        /// The shapes of all touch points on the map.
        /// </summary>
        [JsonInclude] public List<MapEventShape>? Shapes { get; internal set; }

        /// <summary>
        /// The id of the layer the event is attached to.
        /// </summary>
        [JsonInclude] public string? LayerId { get; internal set; }
    }
}
