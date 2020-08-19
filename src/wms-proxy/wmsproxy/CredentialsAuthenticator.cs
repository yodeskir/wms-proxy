using System.Net.Http;
using WMSAuthentication.Interfaces;
using WMSDataAccess.UserManagement;
using wmsShared.Model;
using WMSTools.Interfaces;

namespace WMSAuthentication.WMSProxy
{
    public class CredentialsAuthenticator : ICredentialsAuthenticator
    {
        private readonly ITokenHelper _tokenHelper;
        private readonly IHashHelper _hashHelper;
        private readonly IUserManager _userManager;
        private readonly IHttpRequestMessageHandler _httpRequestMessageHandler;

        public CredentialsAuthenticator(ITokenHelper tokenHelper, IHashHelper hashHelper, IUserManager userManager, IHttpRequestMessageHandler httpRequestMessageHandler)
        {
            _tokenHelper = tokenHelper;
            _hashHelper = hashHelper;
            _userManager = userManager;
            _httpRequestMessageHandler = httpRequestMessageHandler;
        }

        public AuthenticationResult Authenticate(HttpRequestMessage requestMessage)
        {
            //if (!requestMessage.RequestUri.PathAndQuery.Contains("clientid", System.StringComparison.OrdinalIgnoreCase)) {
            //    return new AuthenticationResult { IsAuthenticated = false, ErrorCode = 401, Reason = "You must provide your Client ID." };
            //}
            var credentials = _httpRequestMessageHandler.GetCredentials(requestMessage);
            if (credentials == null)
                return new AuthenticationResult { IsAuthenticated = false, ErrorCode = 401, Reason = "You must provide user and Password." };

            var user = _userManager.GetUser(credentials.UserName);
            if (user == null)
                return new AuthenticationResult { IsAuthenticated = false, ErrorCode = 401, Reason = "User does not exist" };

            var salt = user.salt;
            var hashedPassword = _hashHelper.GetHash(credentials.Password + salt);
            if (user.hashedpassword == hashedPassword)
            {
                var token = _tokenHelper.BuildToken(credentials.UserName, "sitaonair", "wms"); //requestMessage.Headers.GetValues("RemoteAddress")?.FirstOrDefault()
                return new AuthenticationResult { IsAuthenticated = true, Username = credentials.UserName, Token = token, Regenerate = true };
            }

            return new AuthenticationResult { IsAuthenticated = false, ErrorCode = 401, Reason = "Authentication failed. Wrong Password" };
        }
    }
}