namespace wmsShared.Model
{
    public class AuthenticationResult
    {
        public bool IsAuthenticated { get; set; }
        public int ErrorCode { get; set; }
        public string Reason { get; set; } = "";
        public string Username { get; set; }
        public string Token { get; set; }
        public bool Regenerate { get; set; }
    }
}
