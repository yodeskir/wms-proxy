
namespace wmsShared.Model
{
    public class MapEntryRequest
    {
        public int id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string layerid { get; set; }
        public string objectid { get; set; }
        public string objtype { get; set; }
        public MapfileEntry entry { get; set; }
        public bool deleteEntry { get; set; }
        public bool deleteDatasource { get; set; }
    }
}
