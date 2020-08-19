using System;
using System.Linq;
using System.Net.Http;
using WMSTools.Interfaces;

namespace WMSTools
{
    public class TokenHelper : ITokenHelper
    {
        private readonly ICredentialHandler _credentialHandler;
        private readonly IJwtTokenHandler _jwtTokenHandler;

        public TokenHelper(ICredentialHandler credentialHandler, IJwtTokenHandler jwtTokenHandler)
        {
            _credentialHandler = credentialHandler ?? throw new ArgumentNullException(nameof(credentialHandler));
            _jwtTokenHandler = jwtTokenHandler ?? throw new ArgumentNullException(nameof(jwtTokenHandler));
        }

        public string BuildToken(string user, string issuer, string audience)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (issuer == null)
                throw new ArgumentNullException(nameof(issuer));
            if (audience == null)
                throw new ArgumentNullException(nameof(audience));

            var credentials = _credentialHandler.SigningCredentials();

            var writeToken = _jwtTokenHandler.WriteToken(user, issuer, audience, credentials);

            return writeToken;
        }
        public string GetTokenFromAuthTokenHeader(HttpRequestMessage requestMessage)
        {
            if (requestMessage.Headers.TryGetValues("WMS.Auth", out var authHeader))
                return authHeader.FirstOrDefault();
            return null;
        }

        public string GetTokenFromCookieHeader(HttpRequestMessage requestMessage)
        {
            string authString = null;

            if (requestMessage.Headers.TryGetValues("Cookie", out var authHeader))
                authString = authHeader.FirstOrDefault(a => a.StartsWith("WMS.Auth=", StringComparison.InvariantCultureIgnoreCase));

            return authString?.Split(new []{ "WMS.Auth=" }, StringSplitOptions.RemoveEmptyEntries)?[0]?.Split(';')?[0];
        }

    }
}