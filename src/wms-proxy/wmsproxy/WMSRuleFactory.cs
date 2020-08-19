using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WMSAuthentication.Interfaces;
using WMSDataAccess.UserManagement;
using WMSTools;

namespace WMSAuthentication.WMSProxy
{
    public class WMSRuleFactory : IWMSRuleFactory
    {
        private readonly RuntimeOptions _runtimeOptions;
        private readonly ISessionTokenHandler _sessionTokenHandler;
        private readonly IWMSResponseHandler _wmsResponseHandler;
        private readonly IMemoryCache _urlCache;
        private readonly IUserManager _userManager;

        public WMSRuleFactory(IOptions<RuntimeOptions> runtimeOptions, IWMSResponseHandler wmsResponseHandler, ISessionTokenHandler sessionTokenHandler, IUserManager userManager, IMemoryCache urlCache)
        {
            _runtimeOptions = (runtimeOptions ?? throw new ArgumentNullException(nameof(runtimeOptions))).Value;
            _wmsResponseHandler = wmsResponseHandler ?? throw new ArgumentNullException(nameof(wmsResponseHandler));
            _sessionTokenHandler = sessionTokenHandler ?? throw new ArgumentNullException(nameof(sessionTokenHandler));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _urlCache = urlCache ?? throw new ArgumentNullException(nameof(urlCache));
        }

        public IWMSRule Create<T>() where T : IWMSRule, new()
        {
            var wmsRule = new T();

            wmsRule.Initialize(_wmsResponseHandler, _runtimeOptions, _sessionTokenHandler, _userManager, _urlCache);
           
            return wmsRule;
        }
    }
}