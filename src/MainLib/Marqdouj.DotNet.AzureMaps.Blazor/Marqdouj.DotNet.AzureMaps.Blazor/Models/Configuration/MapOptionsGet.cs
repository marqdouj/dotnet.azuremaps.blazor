namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Configuration
{
    /// <summary>
    /// Used when retrieving the current options for the map.
    /// </summary>
    public class MapOptionsGet
    {
        public CameraOptionsGet? Camera { get; set; }
        public ServiceOptions? Service { get; set; }
        public StyleOptions? Style { get; set; }
        public UserInteractionOptions? UserInteraction { get; set; }
    }
}
