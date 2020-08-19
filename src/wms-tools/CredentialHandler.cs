using System.Text;
using Microsoft.IdentityModel.Tokens;
using WMSTools.Interfaces;

namespace WMSTools
{
    public class CredentialHandler : ICredentialHandler
    {
        public SigningCredentials SigningCredentials()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("dflgmjdfkltgu54906dfklgj"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            return credentials;
        }
    }
}