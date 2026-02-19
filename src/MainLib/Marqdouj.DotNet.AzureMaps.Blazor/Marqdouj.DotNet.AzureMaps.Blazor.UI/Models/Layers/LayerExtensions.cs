using Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Services;

namespace Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Layers
{
    public static class LayerExtensions
    {
        public static void SetOptions(this MapLayerDef layerDef, object options)
        {
            switch (layerDef.LayerType)
            {
                case MapLayerType.Bubble:
                    ((BubbleLayerDef)layerDef).Options = (BubbleLayerOptions)options;
                    break;
                case MapLayerType.HeatMap:
                    ((HeatMapLayerDef)layerDef).Options = (HeatMapLayerOptions)options;
                    break;
                case MapLayerType.Image:
                    ((ImageLayerDef)layerDef).Options = (ImageLayerOptions)options;
                    break;
                case MapLayerType.Line:
                    ((LineLayerDef)layerDef).Options = (LineLayerOptions)options;
                    break;
                case MapLayerType.Polygon:
                    ((PolygonLayerDef)layerDef).Options = (PolygonLayerOptions)options;
                    break;
                case MapLayerType.PolygonExtrusion:
                    ((PolygonExtLayerDef)layerDef).Options = (PolygonExtLayerOptions)options;
                    break;
                case MapLayerType.Symbol:
                    ((SymbolLayerDef)layerDef).Options = (SymbolLayerOptions)options;
                    break;
                case MapLayerType.Tile:
                    ((TileLayerDef)layerDef).Options = (TileLayerOptions)options;
                    break;
                case MapLayerType.Unknown:
                default:
                    break;
            }
        }

        public static MapLayerDef GetClone(this MapLayerDef layerDef)
        {
            return layerDef.LayerType switch
            {
                MapLayerType.Bubble => (MapLayerDef)((BubbleLayerDef)layerDef).Clone(),
                MapLayerType.HeatMap => (MapLayerDef)((HeatMapLayerDef)layerDef).Clone(),
                MapLayerType.Image => (MapLayerDef)((ImageLayerDef)layerDef).Clone(),
                MapLayerType.Line => (MapLayerDef)((LineLayerDef)layerDef).Clone(),
                MapLayerType.Polygon => (MapLayerDef)((PolygonLayerDef)layerDef).Clone(),
                MapLayerType.PolygonExtrusion => (MapLayerDef)((PolygonExtLayerDef)layerDef).Clone(),
                MapLayerType.Symbol => (MapLayerDef)((SymbolLayerDef)layerDef).Clone(),
                MapLayerType.Tile => (MapLayerDef)((TileLayerDef)layerDef).Clone(),
                _ => throw layerDef.LayerType.LayerNotSupported(),
            };
        }

        public static MapLayerDef GetLayerDef(this MapLayerType layerType)
        {
            return layerType switch
            {
                MapLayerType.Bubble => new BubbleLayerDef(),
                MapLayerType.HeatMap => new HeatMapLayerDef(),
                MapLayerType.Image => new ImageLayerDef(),
                MapLayerType.Line => new LineLayerDef(),
                MapLayerType.Polygon => new PolygonLayerDef(),
                MapLayerType.PolygonExtrusion => new PolygonExtLayerDef(),
                MapLayerType.Symbol => new SymbolLayerDef(),
                MapLayerType.Tile => new TileLayerDef(),
                _ => throw layerType.LayerNotSupported(),
            };
        }

        public static ILayerUIModel GetLayerDefUIModel(this MapLayerType layerType, IAzureMapsUIXmlService xmlService)
        {
            var layerDef = layerType.GetLayerDef();
            return layerDef.GetLayerDefUIModel(xmlService);
        }

        public static ILayerUIModel GetLayerDefUIModel(this MapLayerDef layerDef, IAzureMapsUIXmlService xmlService)
        {
            return layerDef.LayerType switch
            {
                MapLayerType.Bubble => new BubbleLayerUIModel(xmlService) { Source = (BubbleLayerDef)layerDef },
                MapLayerType.HeatMap => new HeatMapLayerUIModel(xmlService) { Source = (HeatMapLayerDef)layerDef },
                MapLayerType.Image => new ImageLayerUIModel(xmlService) { Source = (ImageLayerDef)layerDef },
                MapLayerType.Line => new LineLayerUIModel(xmlService) { Source = (LineLayerDef)layerDef },
                MapLayerType.Polygon => new PolygonLayerUIModel(xmlService) { Source = (PolygonLayerDef)layerDef },
                MapLayerType.PolygonExtrusion => new PolygonExtLayerUIModel(xmlService) { Source = (PolygonExtLayerDef)layerDef },
                MapLayerType.Symbol => new SymbolLayerUIModel(xmlService) { Source = (SymbolLayerDef)layerDef },
                MapLayerType.Tile => new TileLayerUIModel(xmlService) { Source = (TileLayerDef)layerDef },
                _ => throw layerDef.LayerType.LayerNotSupported(),
            };
        }

        private static NotSupportedException LayerNotSupported(this MapLayerType layerType) => new($"Layer type '{layerType}' is not supported.");
    }
}
