using System;
using System.Collections.Generic;
using System.Reflection;

namespace wmsShared.Model
{
    public class MapDirectivesRuntimeOptions
    {
        public List<MapfileEntry> LAYER { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> CLUSTER { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> RASTER { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> HEATMAP { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> CONTOUR { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> CHART { get; set; } = new List<MapfileEntry>();

        public object this[string propertyName]
        {
            get
            {
                Type myType = typeof(MapDirectivesRuntimeOptions);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                return myPropInfo?.GetValue(this, null);
            }
            set
            {
                Type myType = typeof(MapDirectivesRuntimeOptions);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                myPropInfo.SetValue(this, value, null);

            }

        }
    }
}
