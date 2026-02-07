using Marqdouj.DotNet.AzureMaps.Blazor.Models.Configuration;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Controls;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Events;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Settings;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop
{
    /// <summary>
    /// Manages Azure Maps SDK JSInterop for the Component.
    /// </summary>
    public interface IAzMapsManager
    {
        /// <summary>
        /// Creates an instance of an azure map.
        /// </summary>
        /// <param name="mapId"><see cref="IAzMapInterop.MapId"/></param>
        /// <param name="events"><see cref="MapEventDef"/></param>
        /// <param name="options"><see cref="MapOptions"/></param>
        /// <param name="controls"><see cref="MapControl"/></param>
        /// <param name="logLevel"><see cref="LogLevel"/> used in browser console.</param>
        /// <returns>an instance of <see cref="IAzMapInterop"/> that manages the js interop with the map.</returns>
        /// <exception cref="Exception"></exception>
        Task CreateMap(
            string mapId,
            IEnumerable<MapEventDef>? events = null,
            MapOptions? options = null,
            IEnumerable<MapControl>? controls = null,
            LogLevel logLevel = LogLevel.Information);

        /// <summary>
        /// Retrieves the Azure Maps interop instance associated with the specified map identifier.
        /// </summary>
        /// <param name="mapId">The unique identifier of the map for which to obtain the interop instance. Cannot be null or empty.</param>
        /// <returns>An object that provides interop functionality for the specified Azure Map.</returns>
        /// <exception cref="Exception">Thrown if no interop instance is found for the specified <paramref name="mapId"/>.</exception>
        IAzMapInterop GetInterop(string mapId);

        /// <summary>
        /// Attempts to retrieve the Azure Maps interop instance associated with the specified map identifier.
        /// </summary>
        /// <param name="mapId">The unique identifier of the map for which to retrieve the interop instance. Cannot be null.</param>
        /// <returns>The interop instance associated with the specified map identifier, or null if no instance is found.</returns>
        IAzMapInterop? TryGetInterop(string mapId);

        /// <summary>
        /// Attempts to retrieve the interop instance associated with the specified map identifier.
        /// </summary>
        /// <param name="mapId">The unique identifier of the map for which to retrieve the interop instance. Cannot be null.</param>
        /// <param name="interop">When this method returns, contains the interop instance associated with the specified map identifier, if
        /// found; otherwise, null. This parameter is passed uninitialized.</param>
        /// <returns>true if an interop instance was found for the specified map identifier; otherwise, false.</returns>
        bool TryGetInterop(string mapId, out IAzMapInterop? interop);
    }

    internal sealed class AzMapsManager<T> : IAsyncDisposable, IAzMapsManager where T : ComponentBase
    {
        private readonly AzMapModuleReferences mapReference;
        private readonly Dictionary<string, AzMapInterop> interops = [];
        private readonly MapConfiguration config;
        private readonly ILogger? logger;

        internal AzMapsManager(IJSRuntime jsRuntime, T component, MapConfiguration config, ILogger? logger = null)
        {
            var dotNetRef = DotNetObjectReference.Create(component)
                ?? throw new Exception("Unable to create DotNetObjectReference.");
            mapReference = new(jsRuntime, dotNetRef);
            this.config = config;
            this.logger = logger;
        }

        internal object DotNetRef => (DotNetObjectReference<T>)mapReference.DotNetRef;

        public async Task CreateMap(
            string mapId,
            IEnumerable<MapEventDef>? events = null,
            MapOptions? options = null,
            IEnumerable<MapControl>? controls = null,
            LogLevel logLevel = LogLevel.Information)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(mapId, nameof(mapId));

            mapId = mapId.Trim();

            if (interops.ContainsKey(mapId))
                throw new Exception($"Map instance already exists where mapId = {mapId}");

            var interop = new AzMapInterop(mapId, mapReference, config.Authentication, logLevel);
            await interop.Factory.CreateMap(events, options ?? config.Options, controls);
            interops.Add(mapId, interop);
        }

        public bool TryGetInterop(string mapId, out IAzMapInterop? interop)
        {
            if (interops.TryGetValue(mapId, out var value))
            {
                interop = value;
                return true;
            }

            interop = default;
            return false;
        }

        public IAzMapInterop? TryGetInterop(string mapId)
        {
            return interops.TryGetValue(mapId, out AzMapInterop? value) ? value : null;
        }

        public IAzMapInterop GetInterop(string mapId)
        {
            return TryGetInterop(mapId) ?? throw new Exception($"{nameof(GetInterop)} returned null where mapId = '{mapId}'");
        }

        public async ValueTask DisposeAsync()
        {
            await RemoveMaps();
            ((IDisposable)DotNetRef)?.Dispose();
        }

        private async Task RemoveMaps()
        {
            foreach (var item in interops)
            {
                try
                {
                    await item.Value.Factory.RemoveMap();
                }
                catch (JSDisconnectedException)
                {
                    //no need to go further; disconnected.
                    break;
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex, "RemoveMap failed.");
                }
            }

            interops.Clear();
        }
    }
}
