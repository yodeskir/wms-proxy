using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WMSAuthentication.Interfaces
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage proxyRequest, CancellationToken cancellationToken);
    }
}