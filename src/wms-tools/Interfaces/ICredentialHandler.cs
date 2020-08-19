using Microsoft.IdentityModel.Tokens;

namespace WMSTools.Interfaces
{
    public interface ICredentialHandler
    {
        SigningCredentials SigningCredentials();
    }
}