using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WMSAuthentication.Interfaces
{
    public interface IWMSProxyManager
    {
        Task ProxyRequest(HttpContext context, IWMSRule matchedRule);
    }
}