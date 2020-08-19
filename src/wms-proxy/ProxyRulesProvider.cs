using System.Collections.Generic;
using WMSAuthentication.Interfaces;
using WMSAuthentication.WMSProxy;

namespace WMSAuthentication
{
    public class ProxyRulesProvider : IProxyRulesProvider
    {
        private readonly IWMSRuleFactory _wmsRuleFactory;

        public ProxyRulesProvider(IWMSRuleFactory wmsRuleFactory)
        {
            _wmsRuleFactory = wmsRuleFactory;
        }

        public List<IWMSRule> GetProxyRules()
        {
            var resultRules = new List<IWMSRule>
            {
                _wmsRuleFactory.Create<WMSRule>()
            };
            return resultRules;
        }
    }
}