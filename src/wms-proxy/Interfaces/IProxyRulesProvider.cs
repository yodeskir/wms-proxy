using System.Collections.Generic;

namespace WMSAuthentication.Interfaces
{
    public interface IProxyRulesProvider
    {
        List<IWMSRule> GetProxyRules();
    }
}