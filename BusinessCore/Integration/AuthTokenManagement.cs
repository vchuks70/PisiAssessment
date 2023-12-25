using CoreObject;
using CoreObject.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.Integration
{
    public class AuthTokenManagement
    {
     public static string WriteJwt(User user, AppSettings _appSettings)
            {
                var authClaims = new List<Claim>
                {
                   new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Key));

                var token = new JwtSecurityToken(
                    issuer: _appSettings.Issuer,
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha512)
                );

                return new JwtSecurityTokenHandler().WriteToken(token);

            }

    }
}
