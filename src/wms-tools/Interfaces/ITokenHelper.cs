using System.Net.Http;

namespace WMSTools.Interfaces
{
    public interface ITokenHelper
    {
        string BuildToken(string user, string issuer, string audience);
        string GetTokenFromAuthTokenHeader(HttpRequestMessage requestMessage);
        string GetTokenFromCookieHeader(HttpRequestMessage requestMessage);
    }
}