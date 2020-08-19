
namespace wmsShared.Model
{
    public class ImportViewModel
    {

        public string TargetFilePath { get; set; }
        public LayerType UploadedLayerType { get; set; }
        public string NumberClasses { get; set; }
        public string ColorClasses { get; set; }
        public object SourceFile { get; set; }
        public string MapName { get; set; }
        public string LayerName { get; set; }
        public string DatasourceName { get; set; }
        public string LayerDescription { get; set; }
        public string Projection { get; set; }
        public string IsPublic { get; set; }
        public string UserName { get; set; }
        public string Geometry { get; set; }
        public string ConnectionLayer { get; set; }
        public string Extent { get; set; }
        public bool NeedsRework { get; set; }
        public string Message { get; set; }
        public bool IsValid { get; set; }
    }
}
