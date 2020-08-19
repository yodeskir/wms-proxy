using System;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using WMSAuthentication.Interfaces;

namespace WMSAuthentication.WMSProxy
{
    public class ContextHandler : IContextHandler
    {
        public Uri GetRequestUri(HttpRequest contextRequest)
        {
            if (contextRequest == null)
                throw new ArgumentNullException(nameof(contextRequest));

            var uriString = $"{contextRequest.Scheme}://{contextRequest.Host}{contextRequest.PathBase}{contextRequest.Path}{contextRequest.QueryString}";
            return new Uri(uriString);
        }

        public bool UserIsAuthenticated(IPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Identity.IsAuthenticated;
        }
    }
}