namespace wmsShared.Model
{
    public class Credentials
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public AuthMethod Method => AuthMethod.Basic;
    }
}