using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WMSAuthentication.Interfaces
{
    public interface IWMSProxy
    {
        Task ProxyTheRequest(HttpContext context, HttpRequestMessage proxyRequest, IWMSRule proxyRule);
    }
}