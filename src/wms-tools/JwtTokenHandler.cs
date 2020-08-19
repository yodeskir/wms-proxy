using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using WMSTools.Interfaces;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace WMSTools
{
    public class JwtTokenHandler : IJwtTokenHandler
    {
        private readonly IDateTimeHelper _dateTimeHelper;

        public JwtTokenHandler(IDateTimeHelper dateTimeHelper)
        {
            _dateTimeHelper = dateTimeHelper;
        }
        public string WriteToken(string user, string issuer, string audience, SigningCredentials credentials)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(issuer,
                audience,
                claims,
                expires: _dateTimeHelper.GetDateTimeNow().AddHours(24),
                signingCredentials: credentials);

            var writeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return writeToken;
        }
    }
}