using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using wmsShared.Model;

namespace WMSTools.Interfaces
{
    public interface IHttpRequestMessageHandler
    {
        string GetHeadersHost(HttpRequestMessage proxyRequest);
        HttpRequestMessage GenerateProxyRequest(HttpContext context, Uri uri);
        Credentials GetCredentials(HttpRequestMessage req);
        string GetAuthCookie(HttpRequestMessage requestMessage);
    }
}