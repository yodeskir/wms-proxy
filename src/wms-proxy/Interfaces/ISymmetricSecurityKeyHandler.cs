using Microsoft.IdentityModel.Tokens;

namespace WMSAuthentication.Interfaces
{
    public interface ISymmetricSecurityKeyHandler
    {
        SymmetricSecurityKey SymmetricSecurityKey();
    }
}