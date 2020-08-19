namespace wmsShared.Model
{
    public class NewMapElementResult
    {
        public string ItemId { get; set; }
        public object MapFile { get; set; }
        public StatusCode Status { get; set; }
        public string Message { get; set; }

    }

    public enum StatusCode
    {
        Ok,
        ServerError,
        Unauthorized,
        Warning
    }
}
