using System;
using wmsShared.Interfaces;

namespace wmsShared.Model
{
    [Serializable]
    public class MapfileEntry: IMapfileEntry
    {
        public string id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string setValues { get; set; }
        public string defValue { get; set; }
        public bool showByDefault { get; set; } = false;
        public bool quoteName { get; set; } = false;
        public bool quoteValue { get; set; } = false;
        public bool braketAttr { get; set; } = false;
        public bool includeAsDefault { get; set; } = false;
        public bool canUseAttribute { get; set; } = false;
        public bool readOnly { get; set; } = false;
        public bool hasEnd { get; set; } = false;
        public VALUETYPE valueType { get; set; }
        public bool allowMultiplesInstances { get; set; } = false; 
        public string help { get; set; }

        
    }


    
}
