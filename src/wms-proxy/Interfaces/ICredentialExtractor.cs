using System.Collections.Specialized;
using System.Net.Http.Headers;
using wmsShared.Model;

namespace WMSAuthentication.Interfaces
{
    public interface ICredentialExtractor
    {
        Credentials GetCredentials(NameValueCollection queryString);
        Credentials GetCredentials(string authString);
        string GetAuthString(HttpRequestHeaders headers);
    }
}