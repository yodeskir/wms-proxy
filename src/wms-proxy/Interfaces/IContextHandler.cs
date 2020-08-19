using System;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;

namespace WMSAuthentication.Interfaces
{
    public interface IContextHandler
    {
        Uri GetRequestUri(HttpRequest contextRequest);
        bool UserIsAuthenticated(IPrincipal claimsPrincipal);
    }
}