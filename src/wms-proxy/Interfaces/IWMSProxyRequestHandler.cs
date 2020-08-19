using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace WMSAuthentication.Interfaces
{
    public interface IWMSProxyRequestHandler
    {
        HttpRequestMessage GetProxyRequest(HttpContext context, IWMSRule matchedRule, Uri uri);
    }
}