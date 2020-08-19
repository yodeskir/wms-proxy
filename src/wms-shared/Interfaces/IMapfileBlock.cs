using System;
using System.Collections.Generic;
using wmsShared.Model;

namespace wmsShared.Interfaces
{
    public interface IMapfileBlock: IMapfileEntry
    {
        string endName { get;}
        List<MapfileEntry> entries { get; set; }
        List<IMapfileBlock> blocks { get; set; }
        IMapfileBlock GetNewBlockInstance(string blockName);
        void AddDefaultEntries(LayerType ltype, List<MapfileEntry> allEntries);
        IMapfileBlock Clone();
        string ToString(int tabOrder);
    }
}
