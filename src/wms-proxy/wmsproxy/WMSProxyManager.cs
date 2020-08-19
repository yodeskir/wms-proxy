using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WMSAuthentication.Interfaces;

namespace WMSAuthentication.WMSProxy
{
    public class WMSProxyManager : IWMSProxyManager
    {
        private readonly ILogger _logger;
        private readonly IContextHandler _contextHandler;
        private readonly IWMSProxyRequestHandler _wmsProxyRequestHandler;
        private readonly IWMSProxy _wmsProxy;

        public WMSProxyManager(IWMSProxyRequestHandler wmsProxyRequestHandler, IWMSProxy wmsProxy, ILogger logger, IContextHandler contextHandler)
        {
            _wmsProxyRequestHandler = wmsProxyRequestHandler ?? throw new ArgumentNullException(nameof(wmsProxyRequestHandler));
            _wmsProxy = wmsProxy ?? throw new ArgumentNullException(nameof(wmsProxy));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _contextHandler = contextHandler ?? throw new ArgumentNullException(nameof(contextHandler));
        }

        public async Task ProxyRequest(HttpContext context, IWMSRule matchedRule)
        {
            var uri = _contextHandler.GetRequestUri(context.Request);
            var proxyRequest = _wmsProxyRequestHandler.GetProxyRequest(context, matchedRule, uri);

            try
            {
                await _wmsProxy.ProxyTheRequest(context, proxyRequest, matchedRule);
            }
            catch (HttpRequestException ex)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            }

            _logger.LogDebug($@"Original Url: {proxyRequest.RequestUri.AbsoluteUri}, Proxied Url: {uri}, StatusCode: {context.Response.StatusCode}");
        }
    }
}