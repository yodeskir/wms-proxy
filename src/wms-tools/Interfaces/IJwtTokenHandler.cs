using Microsoft.IdentityModel.Tokens;

namespace WMSTools.Interfaces
{
    public interface IJwtTokenHandler
    {
        string WriteToken(string user, string issuer, string audience, SigningCredentials credentials);
    }
}