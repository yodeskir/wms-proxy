using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using WMSDataAccess.UserManagement;
using WMSTools;

namespace WMSAuthentication.Interfaces
{
    public interface IWMSRule
    {
        bool Matcher(Uri uri);
        void SetSessionToken(HttpRequestMessage httpRequestMessage);
        Task ResponseModifier(HttpResponseMessage httpResponseMessage, HttpContext httpContext);

        void ChangeRequestUri(HttpRequestMessage httpRequestMessage, HttpContext httpContext);

        bool PreProcessResponse { get; }
        bool RequiresAuthentication { get; set; }
        void Initialize(IWMSResponseHandler wmsResponseHandler, RuntimeOptions runtimeOptions, ISessionTokenHandler sessionTokenHandler, IUserManager userManager, IMemoryCache urlCache);
    }
}