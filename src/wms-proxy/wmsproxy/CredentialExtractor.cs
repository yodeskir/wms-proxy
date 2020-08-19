using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using WMSAuthentication.Interfaces;
using wmsShared.Model;

namespace WMSAuthentication.WMSProxy
{
    public class CredentialExtractor : ICredentialExtractor
    {
        public Credentials GetCredentials(string authString)
        {
            if (!string.IsNullOrEmpty(authString))
            {
                string decodeString = Encoding.UTF8.GetString(Convert.FromBase64String(authString));
                var strings = decodeString.Split(':');

                if (strings.Length == 2)
                {
                    var username = strings[0];
                    var password = strings[1];

                    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                        return null;

                    return new Credentials { UserName = username, Password = password };
                }
            }

            return null;
        }
        
        public Credentials GetCredentials(NameValueCollection queryString)
        {
            var username = string.Empty;
            var password = string.Empty;
            foreach (string key in queryString)
            {
                if (key.Equals("username", StringComparison.InvariantCultureIgnoreCase))
                    username = queryString[key];
                if (key.Equals("password", StringComparison.InvariantCultureIgnoreCase))
                    password = queryString[key];
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                    break;
            }

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            return new Credentials { UserName = username, Password = password };
        }

        public string GetAuthString(HttpRequestHeaders headers)
        {
            var authString = "";
            if (headers.TryGetValues("Authorization", out var authHeader))
                authString = authHeader.First();

            if (authHeader != null && !string.IsNullOrEmpty(authString))
                authString = authString.Contains("Basic ") ? authString.Split(' ')[1] : string.Empty;

            return authString;
        }
    }
}