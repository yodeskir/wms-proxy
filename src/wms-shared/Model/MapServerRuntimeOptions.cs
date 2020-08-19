namespace wmsShared.Model
{
    public class MapServerRuntimeOptions
    {
        public string ConnectionString { get;  set; }
        public string MapFileConnectionString { get;  set; }
        public string GlobalMapFilesLocation { get;  set; }
        public string UserMapFilesLocation { get;  set; }
        public string MsErrorFile { get;  set; }
        public string PrjFile { get;  set; }
        public string ImagePath { get;  set; }
        public string ImageUrl { get;  set; }
        public string FontSet { get;  set; }
        public string ShapePath { get;  set; }
    }
}