using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using WMSAuthentication.Interfaces;
using WMSAuthentication.Model;

namespace WMSAuthentication.WMSProxy
{
    public class WMSProxyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WMSProxyOptions _wmsProxyOptions;
        private readonly IContextHandler _contextHandler;
        private readonly IWMSProxyManager _wmsProxyManager;

        public WMSProxyMiddleware(RequestDelegate next, IOptions<WMSProxyOptions> wmsProxyOptions, IContextHandler contextHandler, IWMSProxyManager wmsProxyManager)
        {
            _contextHandler = contextHandler ?? throw new ArgumentNullException(nameof(contextHandler));
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _wmsProxyOptions = wmsProxyOptions?.Value ?? throw new ArgumentNullException(nameof(wmsProxyOptions));
            _wmsProxyManager = wmsProxyManager ?? throw new ArgumentNullException(nameof(wmsProxyManager));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var uri = _contextHandler.GetRequestUri(context.Request);

            var matchedRule = _wmsProxyOptions.ProxyRules.FirstOrDefault(r => r.Matcher(uri));
            if (matchedRule == null)
            {
                await _next(context);
                return;
            }

            if (matchedRule.RequiresAuthentication && !_contextHandler.UserIsAuthenticated(context.User))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            await _wmsProxyManager.ProxyRequest(context, matchedRule);

        }
    }
}