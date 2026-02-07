using Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop.Modules;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Settings;
using Microsoft.Extensions.Logging;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop
{
    /// <summary>
    /// Manages azure maps js interop.
    /// </summary>
    public interface IAzMapInterop
    {
        /// <summary>
        /// CSS id for the map container (div).
        /// </summary>
        string MapId { get; }

        /// <summary>
        /// <see cref="IAzMapConfigurations"/>
        /// </summary>
        IAzMapConfigurations Configurations { get; }
        IAzMapEvents Events { get; }
        IAzMapAnimations Animations { get; }
        IAzMapControls Controls { get; }
        IAzMapFeatures Features { get; }
        IAzMapGeolocations Geolocations { get; }
        IAzMapImageSprites ImageSprites { get; }
        IAzMapLayers Layers { get; }
        IAzMapMarkers Markers { get; }
        IAzMapMercators Mercators { get; }
        IAzMapPopups Popups { get; }
        IAzMapSources Sources { get; }
    }

    internal class AzMapInterop : IAzMapInterop
    {
        internal AzMapInterop(string mapId, AzMapModuleReferences dotNetRef, MapAuthentication authentication, LogLevel logLevel)
        {
            MapId = mapId;
            LogLevel = logLevel;
            Factory = new AzFactory(dotNetRef, authentication, mapId, logLevel);

            Animations = new AzAnimations(dotNetRef, mapId);
            Configurations = new AzConfigurations(dotNetRef, mapId);
            Controls = new AzControls(dotNetRef, mapId);
            Events = new AzEvents(dotNetRef, mapId);
            Features = new AzFeatures(dotNetRef, mapId);
            Geolocations = new AzGeolocations(dotNetRef, mapId);
            ImageSprites = new AzImageSprites(dotNetRef, mapId);
            Layers = new AzLayers(dotNetRef, mapId);
            Markers = new AzMarkers(dotNetRef, mapId);
            Mercators = new AzMercators(dotNetRef.JsRuntime);
            Popups = new AzPopups(dotNetRef, mapId);
            Sources = new AzSources(dotNetRef, mapId);
        }


        internal AzFactory Factory { get; }

        public IAzMapAnimations Animations { get; }
        public IAzMapConfigurations Configurations { get; }
        public IAzMapControls Controls { get; }
        public IAzMapEvents Events { get; }
        public IAzMapFeatures Features { get; }
        public IAzMapGeolocations Geolocations { get; }
        public IAzMapImageSprites ImageSprites { get; }
        public IAzMapLayers Layers { get; }
        public IAzMapMarkers Markers { get; }
        public IAzMapMercators Mercators { get; }
        public IAzMapPopups Popups { get; }
        public IAzMapSources Sources { get; }

        public string MapId { get; }
        public LogLevel LogLevel { get; }
    }
}
