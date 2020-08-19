using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using WMSAuthentication.Interfaces;
using WMSTools.Interfaces;

namespace WMSAuthentication.WMSProxy
{
    public class WMSProxyRequestHandler : IWMSProxyRequestHandler
    {
        private readonly IHttpRequestMessageHandler _httpRequestMessageHandler;

        public WMSProxyRequestHandler(IHttpRequestMessageHandler httpRequestMessageHandler)
        {
            _httpRequestMessageHandler = httpRequestMessageHandler ?? throw new ArgumentNullException(nameof(httpRequestMessageHandler));
        }
        public HttpRequestMessage GetProxyRequest(HttpContext context, IWMSRule matchedRule, Uri uri)
        {
            var requestMessage = _httpRequestMessageHandler.GenerateProxyRequest(context, uri);

            matchedRule.SetSessionToken(requestMessage);

            matchedRule.ChangeRequestUri(requestMessage, context);            

            requestMessage.Headers.Host = _httpRequestMessageHandler.GetHeadersHost(requestMessage);

            return requestMessage;
        }
    }
}