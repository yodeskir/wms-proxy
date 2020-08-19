namespace wmsShared.Model
{
    public enum LayerType
    {
        Point,
        Line,
        Polygon,
        Raster,
        HeatMap,
        Chart,
        Circle,
        Autodetect
    }

    public static class LayerTypeHelper
    {
        public static LayerType GetFinalLayerType(LayerType layerType, string geomname, bool isFinal)
        {
            if (layerType == LayerType.Autodetect)
            {
                switch (geomname)
                {
                    case "point":
                    case "multipoint":
                        return LayerType.Point;
                    case "line":
                    case "linestring":
                    case "multiline":
                    case "multilinestring":
                        return LayerType.Line;
                    case "polygon":
                    case "multipolygon":
                        return LayerType.Polygon;
                    case "raster":
                        return LayerType.Raster;
                    case "heatmap":
                        return LayerType.HeatMap;
                    case "chart":
                        return LayerType.Chart;
                    case "circle":
                        return LayerType.Circle;
                    default:
                        return LayerType.Point;
                }
            }
            return layerType;
        }
    }

}
