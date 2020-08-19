using System;
using System.Net.Http;
using WMSAuthentication.Interfaces;
using wmsShared.Model;
using wmsTools;
using WMSTools.Interfaces;

namespace WMSTools
{
    public class AuthenticatorHandler : IAuthenticatorHandler
    {
        private readonly ITokenAuthenticator _tokenAuthenticator;
        private readonly ICredentialsAuthenticator _credentialsAuthenticator;

        public AuthenticatorHandler(ITokenAuthenticator tokenAuthenticator, ICredentialsAuthenticator credentialsAuthenticator)
        {
            _tokenAuthenticator = tokenAuthenticator ?? throw new ArgumentNullException(nameof(tokenAuthenticator));
            _credentialsAuthenticator = credentialsAuthenticator ?? throw new ArgumentNullException(nameof(credentialsAuthenticator));
        }

        public AuthenticationResult Authenticate(HttpRequestMessage requestMessage)
        {

            var authenticationResult = _tokenAuthenticator.Authenticate(requestMessage);

            if (authenticationResult.IsAuthenticated || requestMessage.Method == HttpMethod.Options)
            {
                ConsoleHelper.Info("Authenticating with TOKEN", ConsoleColor.Cyan);
                return authenticationResult;
            }

            ConsoleHelper.Info("Authenticating with DB Credentials", ConsoleColor.DarkYellow);
            return _credentialsAuthenticator.Authenticate(requestMessage);
        }
    }
}
