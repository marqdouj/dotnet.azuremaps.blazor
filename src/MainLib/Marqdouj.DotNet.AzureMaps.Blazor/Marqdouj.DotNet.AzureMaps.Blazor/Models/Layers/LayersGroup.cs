using Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop;
using System.Collections.ObjectModel;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers
{
    /// <summary>
    /// Represents a group of map layers/events that can be managed and added to or removed from a map as a single unit.
    /// </summary>
    /// <remarks>Use this class to organize related map layers and perform batch operations, such as adding or
    /// removing all layers in the group from a map. The group maintains a read-only collection of its layers and
    /// provides methods to retrieve layers by type. The state of whether the layers have been added to the map is
    /// tracked by the LayersAdded property.</remarks>
    public class LayersGroup
    {
        private readonly List<LayerEventsGroup> layers;

        public LayersGroup(IEnumerable<LayerEventsGroup> layers)
        {
            this.layers = [.. layers];
            Layers = new ReadOnlyCollection<LayerEventsGroup>(this.layers);
        }

        public IReadOnlyCollection<LayerEventsGroup> Layers  { get; }

        /// <summary>
        /// Retrieves the first layer group that matches the specified map layer type.
        /// </summary>
        /// <param name="layerType">The type of map layer to search for within the collection.</param>
        /// <returns>A <see cref="LayerEventsGroup"/> that matches the specified layer type, or <see langword="null"/> if no
        /// matching layer is found.</returns>
        public LayerEventsGroup? GetLayer(MapLayerType layerType)
        {
            return layers.FirstOrDefault(e => e.LayerType == layerType);
        }

        /// <summary>
        /// Retrieves a list of layer event groups that match the specified map layer type.
        /// </summary>
        /// <param name="layerType">The type of map layer to filter by. Only layer event groups whose associated layer matches this type are
        /// included in the result.</param>
        /// <returns>A list of <see cref="LayerEventsGroup"/> objects corresponding to the specified layer type. The list is
        /// empty if no matching layers are found.</returns>
        public List<LayerEventsGroup> GetLayers(MapLayerType layerType)
        {
            return [.. layers.Where(e => e.LayerType == layerType)];
        }

        /// <summary>
        /// Indicates if the layers have been added to the map.
        /// </summary>
        public bool LayersAdded { get; private set; }

        /// <summary>
        /// Adds the layers to the map.
        /// </summary>
        /// <param name="mapInterop"></param>
        /// <returns></returns>
        public async Task AddLayers(IAzMapInterop mapInterop)
        {
            if (LayersAdded) return;
            await mapInterop.Layers.AddGroups(layers);
            LayersAdded = true;
        }

        /// <summary>
        /// Removes the layers from the map.
        /// </summary>
        /// <param name="mapInterop"></param>
        /// <returns></returns>
        public async Task RemoveLayers(IAzMapInterop mapInterop)
        {
            if (!LayersAdded) return;
            await mapInterop.Layers.Remove(layers.Select(e => e.Layer));
            LayersAdded = false;
        }

        public async Task ClearLayers(IAzMapInterop mapInterop)
        {
            await mapInterop.Sources.Clear(layers.Select(e => e.Layer.DataSource));
        }
    }
}
