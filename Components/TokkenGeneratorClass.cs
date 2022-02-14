using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SjxLogistics.Components
{
    public class TokkenGeneratorClass
    {
        public string TokenGenerator(string accessKey, string audiance, string issuer, double expiration, IEnumerable<Claim> claims = null)
        {

            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(accessKey));
            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                audiance,
                issuer,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(expiration),
                signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
