using System.Net.Http;
using wmsShared.Model;

namespace WMSTools.Interfaces
{
    public interface ITokenAuthenticator
    {
        AuthenticationResult Authenticate(HttpRequestMessage requestMessage);
    }
}