using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using WMSAuthentication.Interfaces;
using WMSAuthentication.Model;
using wmsShared.Model;
using WMSTools.Interfaces;

namespace WMSAuthentication.WMSProxy
{
    public class HttpRequestMessageHandler : IHttpRequestMessageHandler
    {
        private readonly WMSProxyOptions _options;
        readonly IHashHelper _hashHelper;
        readonly ITokenHelper _tokenHelper;
        readonly ICredentialExtractor _credentialExtractor;

        public HttpRequestMessageHandler(IOptions<WMSProxyOptions> options, IHashHelper hashHelper, ICredentialExtractor credentialExtractor, ITokenHelper tokenHelper)
        {
            _options = options.Value;
            _hashHelper = hashHelper;
            _credentialExtractor = credentialExtractor;
            _tokenHelper = tokenHelper;
        }

        public string GetHeadersHost(HttpRequestMessage proxyRequest)
        {
            return proxyRequest.RequestUri.IsDefaultPort ? proxyRequest.RequestUri.Host : $"{proxyRequest.RequestUri.Host}:{proxyRequest.RequestUri.Port}";
        }

        public Credentials GetCredentials(HttpRequestMessage req)
        {
            var authString = _credentialExtractor.GetAuthString(req.Headers);

            if (!String.IsNullOrEmpty(authString))
                return _credentialExtractor.GetCredentials(authString);
            else
                return null;

            //var queryString = HttpUtility.ParseQueryString(req.RequestUri.AbsoluteUri);

            //return _credentialExtractor.GetCredentials(queryString);
        }

        public string GetAuthCookie(HttpRequestMessage requestMessage)
        {
            if (requestMessage.RequestUri.PathAndQuery.ToLowerInvariant().Contains("REQUEST=GetCapabilities".ToLowerInvariant()))
                return string.Empty;

            var token = _tokenHelper.GetTokenFromCookieHeader(requestMessage);

            if (String.IsNullOrEmpty(token))
                token = _tokenHelper.GetTokenFromAuthTokenHeader(requestMessage);

            if (String.IsNullOrEmpty(token))
                return String.Empty;

           return _hashHelper.Base64Decode(token);
        }

        public HttpRequestMessage GenerateProxyRequest(HttpContext context, Uri uri)
        {
            var requestMessage = new HttpRequestMessage(new HttpMethod(context.Request.Method), uri)
            {
                Content = GetStreamContent(context)
            };

            foreach (var header in context.Request.Headers)
                if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
                    requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());

            if (_options.AddForwardedHeader)
            {
                requestMessage.Headers.TryAddWithoutValidation("Forwarded", $"for={context.Connection.RemoteIpAddress}");
                requestMessage.Headers.TryAddWithoutValidation("Forwarded", $"host={requestMessage.Headers.Host}");
                requestMessage.Headers.TryAddWithoutValidation("Forwarded", $"proto={(context.Request.IsHttps ? "https" : "http")}");
            }

            return requestMessage;
        }

        private StreamContent GetStreamContent(HttpContext context)
        {
            var requestMethod = context.Request.Method;
            if (HttpMethods.IsGet(requestMethod) 
                || HttpMethods.IsHead(requestMethod) 
                || HttpMethods.IsDelete(requestMethod) 
                || HttpMethods.IsTrace(requestMethod))
                return null;

            return new StreamContent(context.Request.Body);
        }
    }
}