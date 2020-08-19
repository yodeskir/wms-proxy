using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using WMSAuthentication.Interfaces;
using WMSAuthentication.Model;

namespace WMSAuthentication.WMSProxy
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;

        private HttpCompletionOption _httpCompletionOption = HttpCompletionOption.ResponseHeadersRead;

        public HttpClientWrapper(IOptions<WMSProxyOptions> wmsProxyOptions)
        {
            if (wmsProxyOptions?.Value == null)
                throw new ArgumentNullException(nameof(wmsProxyOptions));

            var httpMessageHandler = wmsProxyOptions.Value.BackChannelMessageHandler ?? new HttpClientHandler { AllowAutoRedirect = wmsProxyOptions.Value.FollowRedirects };
            _httpClient = new HttpClient(httpMessageHandler);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage proxyRequest, CancellationToken cancellationToken)
        {
            return await _httpClient.SendAsync(proxyRequest, _httpCompletionOption, cancellationToken);
        }
    }
}