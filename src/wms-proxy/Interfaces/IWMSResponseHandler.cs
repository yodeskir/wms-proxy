using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using wmsShared.Model;

namespace WMSAuthentication.Interfaces
{
    public interface IWMSResponseHandler
    {
        Task SetAuthenticatedResponse(HttpResponseMessage httpResponseMessage, HttpContext httpContext, AuthenticationResult authenticationResult);
        Task SetNonAuthenticatedResponse(HttpContext httpContext, AuthenticationResult authenticationResult);
        void PrepareHeader(HttpResponseMessage httpResponseMessage, HttpContext httpContext);
        Task CopyResponse(HttpResponseMessage httpResponseMessage, HttpContext httpContext);
    }
}