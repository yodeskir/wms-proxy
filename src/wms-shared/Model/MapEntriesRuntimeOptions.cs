using System;
using System.Collections.Generic;
using System.Reflection;

namespace wmsShared.Model
{
    public class MapEntriesRuntimeOptions
    {
        public string BlockNames { get; set; }
        public List<MapfileEntry> MAP { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> WEB { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> METADATA { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> PROJECTION { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> LAYER { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> SCALEBAR { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> CLASS { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> STYLE { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> LABEL { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> COMPOSITE { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> CLUSTER { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> FEATURE { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> GRID { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> LEADER { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> LEGEND { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> OUTPUTFORMAT { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> PATTERN { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> POINTS { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> SYMBOL { get; set; } = new List<MapfileEntry>();
        public List<MapfileEntry> VALIDATION { get; set; } = new List<MapfileEntry>();

        public object this[string propertyName]
        {
            get
            {
                Type myType = typeof(MapEntriesRuntimeOptions);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                return myPropInfo?.GetValue(this, null);
            }
            set
            {
                Type myType = typeof(MapEntriesRuntimeOptions);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                myPropInfo.SetValue(this, value, null);

            }

        }
    }
}
