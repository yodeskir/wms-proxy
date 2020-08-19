using System.Net.Http;
using Microsoft.AspNetCore.Http;
using wmsShared.Model;

namespace WMSAuthentication.Interfaces
{
    public interface ISessionTokenHandler
    {
        void SetSessionToken(HttpRequestMessage httpRequestMessage);
        AuthenticationResult GetSessionToken(HttpContext httpContext);
    }
}