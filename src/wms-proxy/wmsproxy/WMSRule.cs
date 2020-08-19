using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;
using WMSAuthentication.Interfaces;
using WMSAuthentication.Model;
using WMSDataAccess.UserManagement;
using wmsTools;
using WMSTools;

namespace WMSAuthentication.WMSProxy
{
    public class WMSRule : IWMSRule
    {
        RuntimeOptions _runtimeOptions;
        ISessionTokenHandler _sessionTokenHandler;
        IWMSResponseHandler _wmsResponseHandler;
        IMemoryCache _urlCache;
        IUserManager _userManager;

        public void Initialize(IWMSResponseHandler wmsResponseHandler, RuntimeOptions runtimeOptions, ISessionTokenHandler sessionTokenHandler, IUserManager userManager, IMemoryCache urlCache)
        {
            _wmsResponseHandler = wmsResponseHandler ?? throw new ArgumentNullException(nameof(wmsResponseHandler));
            _runtimeOptions = runtimeOptions ?? throw new ArgumentNullException(nameof(runtimeOptions));
            _sessionTokenHandler = sessionTokenHandler ?? throw new ArgumentNullException(nameof(sessionTokenHandler));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _urlCache = urlCache ?? throw new ArgumentNullException(nameof(urlCache));

        }

        public bool Matcher(Uri uri)
        {
            return _runtimeOptions.AllowedUrls.Any(u => uri.AbsoluteUri.Contains(u));
        }

        public void SetSessionToken(HttpRequestMessage httpRequestMessage)
        {
            _sessionTokenHandler.SetSessionToken(httpRequestMessage);
        }

        public void ChangeRequestUri(HttpRequestMessage httpRequestMessage, HttpContext httpContext)
        {
            try
            {
                var uri = new UriBuilder(httpRequestMessage.RequestUri)
                {
                    Scheme = Uri.UriSchemeHttp,
                    Host = _runtimeOptions.MapServerHost,
                    Port = _runtimeOptions.ProxyToPort
                };

                var authenticationResult = _sessionTokenHandler.GetSessionToken(httpContext);
                var newUri = GetReplacedSegment(ref uri, authenticationResult.Username, _userManager);
                httpRequestMessage.RequestUri = newUri;
            }
            catch(Exception ex)
            {
                ConsoleHelper.Critical($"ERROR Rewriting URL:\n {ex.Message}", ConsoleColor.Red);
            }
        }

        public async Task ResponseModifier(HttpResponseMessage httpResponseMessage, HttpContext httpContext)
        {
            httpContext.Response.Headers[HeaderNames.AccessControlAllowOrigin] = "*";
            httpContext.Response.Headers[HeaderNames.AccessControlAllowCredentials] = "true";
            httpContext.Response.Headers[HeaderNames.AccessControlAllowHeaders] = "*";
            httpContext.Response.Headers[HeaderNames.AccessControlAllowMethods] = "*";

            var authenticationResult = _sessionTokenHandler.GetSessionToken(httpContext);
            if (!authenticationResult.IsAuthenticated)
                await _wmsResponseHandler.SetNonAuthenticatedResponse(httpContext, authenticationResult);
            else
            {
                if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    _wmsResponseHandler.PrepareHeader(httpResponseMessage, httpContext);
                    await _wmsResponseHandler.SetAuthenticatedResponse(httpResponseMessage, httpContext, authenticationResult);

                }
                else
                {
                    await _wmsResponseHandler.CopyResponse(httpResponseMessage, httpContext);
                }
            }
        }

        public bool PreProcessResponse => false;

        public bool RequiresAuthentication { get; set; }

        private Uri GetReplacedSegment(ref UriBuilder uribuilder, string username, IUserManager userManager)
        {
            Dictionary<string, string> cachedUrls = (Dictionary<string, string>)_urlCache.Get($"{username}_{CacheEntries.URLMapEntry}");
            var foundMap = (cachedUrls != null);
            if (foundMap)
            {
                return GetPathAndQuery(cachedUrls, ref uribuilder);
            }
            else
            {
                return UpdateCacheUrlsFromDB(username, _urlCache, userManager, ref uribuilder);
            }
        }

        private Uri GetPathAndQuery(Dictionary<string, string> cachedUrls, ref UriBuilder uribuilder)
        {
            foreach (var item in cachedUrls)
            {
                var q = item.Value.Split('?');
                if (uribuilder.Path.Contains(item.Key))
                {
                    uribuilder.Path = uribuilder.Path.Replace(item.Key, q[0]);
                    uribuilder.Query += string.Format("&{0}", q?[1]);
                    break;
                }
            }
            return uribuilder.Uri;
        }
        private Uri UpdateCacheUrlsFromDB(string username, IMemoryCache cache, IUserManager userManager, ref UriBuilder uribuilder)
        {
            var maps = userManager.GetUserMaps(username);
            
            var mapurl = new Dictionary<string, string>();
            foreach (var m in maps)
            {
                var key = m.mapfile;
                if (!mapurl.ContainsKey(key))
                {
                    var mapfile = Path.Combine(_runtimeOptions.MapFilesDir, username, $"{m.mapfile}.map");
                    mapurl.Add(key, $"{_runtimeOptions.MapServerName}?map={mapfile}");
                }
            }
            if (mapurl.Count > 0)
            {
                cache.Set($"{username}_{CacheEntries.URLMapEntry}", mapurl);
            }
            return GetPathAndQuery(mapurl, ref uribuilder);
        }
    }
}