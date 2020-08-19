using System.Net.Http;
using wmsShared.Model;

namespace WMSTools.Interfaces
{
    public interface IAuthenticatorHandler
    {
        AuthenticationResult Authenticate(HttpRequestMessage requestMessage);
    }
}