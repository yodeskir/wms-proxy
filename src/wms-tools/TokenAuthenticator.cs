using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using wmsShared.Model;
using WMSTools.Interfaces;


namespace WMSTools
{
    public class TokenAuthenticator : ITokenAuthenticator
    {
        private readonly IHttpRequestMessageHandler _httpRequestMessageHandler;

        public TokenAuthenticator(IHttpRequestMessageHandler httpRequestMessageHandler)
        {
            _httpRequestMessageHandler = httpRequestMessageHandler ?? throw new ArgumentNullException(nameof(httpRequestMessageHandler));
        }

        public AuthenticationResult Authenticate(HttpRequestMessage requestMessage)
        {
            AuthenticationResult result = new AuthenticationResult();
            var requestToken = _httpRequestMessageHandler.GetAuthCookie(requestMessage);

            if (string.IsNullOrEmpty(requestToken))
                return result;

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(requestToken);

            //if (!HasRemoteAccess(jwt, requestMessage))
            //    return result;

            var diffDate = DateTime.Now.Subtract(jwt.ValidTo);

            if (diffDate.TotalHours < 24)
                result = new AuthenticationResult { IsAuthenticated = true, Username = jwt.Payload.Values.First().ToString(), Token = requestToken, Regenerate = false };

            return result;
        }

        private bool HasRemoteAccess(JwtSecurityToken jwt, HttpRequestMessage requestMessage)
        {
            return !requestMessage.Headers.TryGetValues("RemoteAddress", out var remoteAddress) || jwt.Issuer.Equals(remoteAddress.FirstOrDefault());
        }
    }
}