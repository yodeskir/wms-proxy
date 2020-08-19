using System.Net.Http;
using wmsShared.Model;

namespace WMSAuthentication.Interfaces
{
    public interface ICredentialsAuthenticator
    {
        AuthenticationResult Authenticate(HttpRequestMessage requestMessage);
    }
}