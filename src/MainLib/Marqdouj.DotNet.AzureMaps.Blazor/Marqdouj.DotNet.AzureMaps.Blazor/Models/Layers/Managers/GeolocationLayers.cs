using Marqdouj.DotNet.AzureMaps.Blazor.Models.Events;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers.Managers
{
    public class GeolocationLayers
    {
        private readonly SymbolLayerDef positionDef;
        private readonly PolygonLayerDef accuracyDef;
        private readonly DataSourceDef dataSourceDef = new();
        private readonly LayersGroup layersGroup;

        public GeolocationLayers(IEnumerable<MapEventDef>? positionEvents = null, IEnumerable<MapEventDef>? accuracyEvents = null)
        {
            positionDef = new SymbolLayerDef() { DataSource = dataSourceDef };
            accuracyDef = new PolygonLayerDef() { DataSource = dataSourceDef };

            //Render Point or MultiPoints in this layer.
            positionDef.Options ??= new();
            positionDef.Options.Filter = new List<object>
                {
                    "any",
                    new List<object> { "==", new List<string> { "geometry-type" }, "Point" },
                    new List<object> { "==", new List<string> { "geometry-type" }, "MultiPoint" }
                };

            accuracyDef.Options ??= new();
            accuracyDef.Options.FillColor = "rgba(0, 153, 255, 0.5)";

            var positionGroup = new LayerEventsGroup(positionDef, positionEvents);
            var accuracyGroup = new LayerEventsGroup(accuracyDef, accuracyEvents);
            layersGroup = new([positionGroup, accuracyGroup]);
        }

        /// <summary>
        /// Options for the Accuracy layer. Any changes must bed made before adding the layers to the map.
        /// </summary>
        public PolygonLayerOptions AccuracyOptions => accuracyDef.Options!;

        /// <summary>
        /// Options for the Position layer. Any changes must bed made before adding the layers to the map.
        /// </summary>
        public SymbolLayerOptions PositionOptions => positionDef.Options!;

        /// <summary>
        /// Indicates if the layers have been added to the map.
        /// </summary>
        public bool LayersAdded => layersGroup.LayersAdded;

        /// <summary>
        /// Adds the layers to the map.
        /// </summary>
        /// <param name="mapInterop"></param>
        public async Task AddLayers(IAzMapInterop mapInterop) => await layersGroup.AddLayers(mapInterop);

        /// <summary>
        /// Removes the layers from the map.
        /// </summary>
        /// <param name="mapInterop"></param>
        public async Task RemoveLayers(IAzMapInterop mapInterop) => await layersGroup.RemoveLayers(mapInterop);

        /// <summary>
        /// Adds a map feature representing the specified geographic position to the map and optionally includes
        /// an accuracy indicator for the position.
        /// </summary>
        /// <remarks>If showAccuracy is set to true and a position includes accuracy information, an
        /// additional feature representing the accuracy (such as a circle) is added for that position. The returned
        /// list includes both the position features and any associated accuracy features.</remarks>
        /// <param name="mapInterop">An object that provides interop functionality for interacting with Azure Maps.</param>
        /// <param name="position">A geographic position to add as feature on the map.</param>
        /// <param name="showAccuracy">true to add accuracy indicators (such as circles) for positions that include accuracy information;
        /// otherwise, false. The default is true.</param>
        /// <returns>A MapFeatureDef object representing the added position and, if applicable, it's accuracy
        /// indicator.</returns>
        public async Task<MapFeatureDef> AddPosition(IAzMapInterop mapInterop, Position position, bool showAccuracy = true)
            => (await AddPositions(mapInterop, [position], showAccuracy)).First();

        /// <summary>
        /// Adds map features representing the specified geographic positions to the map and optionally includes
        /// accuracy indicators for each position.
        /// </summary>
        /// <remarks>If showAccuracy is set to true and a position includes accuracy information, an
        /// additional feature representing the accuracy (such as a circle) is added for that position. The returned
        /// list includes both the position features and any associated accuracy features.</remarks>
        /// <param name="mapInterop">An object that provides interop functionality for interacting with Azure Maps.</param>
        /// <param name="positions">A collection of geographic positions to add as features on the map.</param>
        /// <param name="showAccuracy">true to add accuracy indicators (such as circles) for positions that include accuracy information;
        /// otherwise, false. The default is true.</param>
        /// <returns>A list of MapFeatureDef objects representing the added positions and, if applicable, their accuracy
        /// indicators.</returns>
        public async Task<List<MapFeatureDef>> AddPositions(IAzMapInterop mapInterop, IEnumerable<Position> positions, bool showAccuracy = true)
        {
            var features = new List<MapFeatureDef>();

            foreach (var position in positions)
            {
                var point = new Point(new Position(position.Longitude, position.Latitude));
                var pointDef = new MapFeatureDef(point) { AsShape = true };
                pointDef.Properties ??= [];
                pointDef.Properties.Add("geolocationType", "position");

                features.Add(pointDef);

                if (showAccuracy && position.Accuracy is not null)
                {
                    var accuracyDef = new MapFeatureDef(point);
                    accuracyDef.Properties ??= [];
                    accuracyDef.Properties.Add("geolocationType", "accuracy");
                    accuracyDef.Properties.Add("subType", "Circle");
                    accuracyDef.Properties.Add("radius", position.Accuracy);
                    features.Add(accuracyDef);
                }
            }

            await mapInterop.Features.Add(features, dataSourceDef.Id);

            return features;
        }

        /// <summary>
        /// Clears the datasource for the Geolocation layers.
        /// </summary>
        /// <param name="mapInterop"></param>
        /// <returns></returns>
        public async Task Clear(IAzMapInterop mapInterop)
        {
            if (LayersAdded)
                await mapInterop.Sources.Clear(dataSourceDef);
        }
    }
}
