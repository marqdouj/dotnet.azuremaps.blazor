namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Configuration
{
    /// <summary>
    /// Used when setting the current options for the map.
    /// </summary>
    public class MapOptionsSet
    {
        public CameraOptionsSet? Camera { get; set; }
        public ServiceOptions? Service { get; set; }
        public StyleOptions? Style { get; set; }
        public UserInteractionOptions? UserInteraction { get; set; }

        internal MapOptions ToCreateMapOptions()
        {
            return new MapOptions
            {
                Camera = Camera?.Camera,
                CameraBounds = Camera?.CameraBounds,
                Service = Service,
                Style = Style,
                UserInteraction = UserInteraction
            };
        }
    }
}
