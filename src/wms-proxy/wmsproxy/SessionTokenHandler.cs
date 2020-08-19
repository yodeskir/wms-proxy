using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WMSAuthentication.Interfaces;
using wmsShared.Model;
using WMSTools;
using WMSTools.Interfaces;

namespace WMSAuthentication.WMSProxy
{
    public class SessionTokenHandler : ISessionTokenHandler
    {
        private IHttpContextAccessor _httpContextAccessor;
        private IAuthenticatorHandler _authenticatorHandler;
        private RuntimeOptions _runtimeOptions;

        public SessionTokenHandler(IHttpContextAccessor httpContextAccessor, IAuthenticatorHandler authenticatorHandler, IOptions<RuntimeOptions> runtimeOptions)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _authenticatorHandler = authenticatorHandler ?? throw new ArgumentNullException(nameof(authenticatorHandler));

            if (runtimeOptions == null)
                throw new ArgumentNullException(nameof(runtimeOptions));

            _runtimeOptions = runtimeOptions.Value;
        }

        public void SetSessionToken(HttpRequestMessage httpRequestMessage)
        {
            var clientIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var authenticationResult = _authenticatorHandler.Authenticate(httpRequestMessage);
            var authResultSerialized = JsonConvert.SerializeObject(authenticationResult);
            _httpContextAccessor.HttpContext.Session.SetString($"{_runtimeOptions.SessionToken}{clientIp}", authResultSerialized);
        }

        public AuthenticationResult GetSessionToken(HttpContext httpContext)
        {
            var clientIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var value = httpContext.Session.GetString($"{_runtimeOptions.SessionToken}{clientIp}");
            return value == null ? default(AuthenticationResult) : JsonConvert.DeserializeObject<AuthenticationResult>(value);
        }
    }
}