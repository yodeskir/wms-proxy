using WMSAuthentication.Model;

namespace WMSAuthentication.Interfaces
{
    public interface IReverseProxyHandler
    {
        WMSProxyOptions GetProxyOptions();
    }
}