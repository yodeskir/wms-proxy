using System;
using System.Collections.Generic;
using System.Text;

namespace wmsShared.Model
{
    public class LayerDataResultModel
    {
        public List<string> Fields { get; set; } = new List<string>();
        public List<object> Data { get; set; } = new List<object>();
    }
}
