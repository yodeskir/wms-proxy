
namespace wmsShared.Model
{
    public class ImportModelResult
    {

        public LayerType FinalLayerType { get; set; } 
        public string LayerName { get; set; }
        public string UserName { get; set; }
        public string Geometry { get; set; }
        public bool NeedsRework { get; set; }
        public string Message { get; set; }
        public bool IsValid { get; set; }
        public object MapFile { get; set; }
    }
}
