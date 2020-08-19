using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using WMSAuthentication.Interfaces;
using wmsShared.Model;
using WMSTools;
using WMSTools.Interfaces;

namespace WMSAuthentication.WMSProxy
{
    public class WMSResponseHandler : IWMSResponseHandler
    {
        readonly RuntimeOptions _runtimeOptions;
        readonly IHashHelper _hashHelper;

        public WMSResponseHandler(IHashHelper hashHelper, IOptions<RuntimeOptions> runtimeOptions)
        {
            _hashHelper = hashHelper ?? throw new ArgumentNullException(nameof(hashHelper));

            if (runtimeOptions == null)
                throw new ArgumentNullException(nameof(runtimeOptions));

            _runtimeOptions = runtimeOptions.Value;
        }

        public async Task SetAuthenticatedResponse(HttpResponseMessage httpResponseMessage, HttpContext httpContext, AuthenticationResult authenticationResult)
        {
            if (authenticationResult.Regenerate)
                httpContext.Response.Headers.Add(_runtimeOptions.SetCookie, $"{_runtimeOptions.CookieToken}{_hashHelper.Base64Encode(authenticationResult.Token)}");

            if (httpResponseMessage.Content != null)
            {
                foreach (var contentHeader in httpResponseMessage.Content.Headers)
                    httpContext.Response.Headers[contentHeader.Key] = contentHeader.Value.ToArray();

                httpContext.Response.Headers[HeaderNames.Expires] = DateTime.Now.AddSeconds(_runtimeOptions.CacheControlMaxAgeInASeconds).ToLongDateString();
                httpContext.Response.Headers[HeaderNames.CacheControl] = $"public,max-age={_runtimeOptions.CacheControlMaxAgeInASeconds}";
                httpContext.Response.Headers[HeaderNames.Origin] = httpResponseMessage.RequestMessage.RequestUri.AbsoluteUri;
                await CopyResponse(httpResponseMessage, httpContext);
            }
        }

        public async Task CopyResponse(HttpResponseMessage httpResponseMessage, HttpContext httpContext)
        {
            await httpResponseMessage.Content.CopyToAsync(httpContext.Response.Body);
        }

        public async Task SetNonAuthenticatedResponse(HttpContext httpContext, AuthenticationResult authenticationResult)
        {
            var data = Encoding.UTF8.GetBytes(authenticationResult?.Reason);

            httpContext.Response.StatusCode = authenticationResult.ErrorCode;
            httpContext.Response.Headers.Add("StatusReason", authenticationResult.Reason);
            httpContext.Response.ContentLength = data.Length;
            await httpContext.Response.Body.WriteAsync(data, 0, data.Length);
        }

        public void PrepareHeader(HttpResponseMessage httpResponseMessage, HttpContext httpContext)
        {
            foreach (var header in httpResponseMessage.Headers)
                httpContext.Response.Headers[header.Key] = header.Value.ToArray();

            // SendAsync removes chunking from the response. 
            // This removes the header so it doesn't expect a chunked response.
            httpContext.Response.Headers.Remove("transfer-encoding");
        }
    }
}