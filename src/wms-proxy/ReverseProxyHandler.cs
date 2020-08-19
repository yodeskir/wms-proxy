using WMSAuthentication.Interfaces;
using WMSAuthentication.Model;

namespace WMSAuthentication
{
    public class ReverseProxyHandler : IReverseProxyHandler
    {
        private readonly IProxyRulesProvider _proxyRulesProvider;

        public ReverseProxyHandler(IProxyRulesProvider proxyRulesProvider)
        {
            _proxyRulesProvider = proxyRulesProvider;
        }

        public WMSProxyOptions GetProxyOptions()
        {
            var proxyOptions = new WMSProxyOptions
            {
                ProxyRules = _proxyRulesProvider.GetProxyRules(),
                FollowRedirects = false
            };
            return proxyOptions;
        }
    }
}