
namespace wmsShared.Model
{
    public class LayerDataRequestModel
    {
        public int? RowStart { get; set; }
        public int? RowEnd { get; set; }
        public string FilterBy { get; set; }
        public string FilterOpType { get; set; }
        public string FilterValue { get; set; }
    }
}
