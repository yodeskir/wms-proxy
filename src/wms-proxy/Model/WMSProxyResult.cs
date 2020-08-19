using System;

namespace WMSAuthentication.Model
{
    public class WMSProxiedResult
    {
        public WMSProxyStatus ProxyStatus { get; set; }
        public int HttpStatusCode { get; set; }
        public Uri OriginalUri { get; set; }
        public Uri ProxiedUri { get; set; }
    }
}