using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WMSAuthentication.Interfaces;

namespace WMSAuthentication.WMSProxy
{
    public class WMSProxy : IWMSProxy
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly IWMSMessageProcessor _wmsMessageProcessor;

        public WMSProxy(IHttpClientWrapper httpClientWrapper, IWMSMessageProcessor wmsMessageProcessor)
        {
            _httpClientWrapper = httpClientWrapper ?? throw new ArgumentNullException(nameof(httpClientWrapper));
            _wmsMessageProcessor = wmsMessageProcessor ?? throw new ArgumentNullException(nameof(wmsMessageProcessor));
        }

        public async Task ProxyTheRequest(HttpContext context, HttpRequestMessage proxyRequest, IWMSRule proxyRule)
        {
            try
            {
                using (var responseMessage = await _httpClientWrapper.SendAsync(proxyRequest, context.RequestAborted))
                {
                    if (proxyRule.PreProcessResponse)
                        await _wmsMessageProcessor.PreProcess(context, responseMessage);

                    await proxyRule.ResponseModifier(responseMessage, context);
                }
            }
            catch(Exception ex) {
            }
        }
    }
}