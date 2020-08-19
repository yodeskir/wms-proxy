using System.Collections.Generic;

namespace WMSTools
{
    public class RuntimeOptions
    {
        public string ConnectionString { get; set; }
        public int ProxyToPort { get; set; }
        public List<string> AllowedUrls { get; set; }
        public string SessionToken { get; set; }
        public string CookieToken { get; set; }
        public string SetCookie { get; set; }
        public int CacheControlMaxAgeInASeconds { get; set; }
        public string MapServerHost { get; set; }

        public string MapFilesDir { get; set; }
        public string MapServerName { get; set; }
    }
}