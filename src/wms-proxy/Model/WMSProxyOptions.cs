using System.Collections.Generic;
using System.Net.Http;
using WMSAuthentication.Interfaces;

namespace WMSAuthentication.Model
{
    public class WMSProxyOptions
    {
        public List<IWMSRule> ProxyRules { get; set; } = new List<IWMSRule>();
        public HttpMessageHandler BackChannelMessageHandler { get; set; }
        public bool FollowRedirects { get; set; } = true;
        public bool AddForwardedHeader { get; set; } = false;
    }
}