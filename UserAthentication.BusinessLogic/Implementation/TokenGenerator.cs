using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserAthentication.Model;

namespace UserAthentication
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<Users> _userManager;

        public TokenGenerator(IConfiguration configuration, UserManager<Users> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<string> GetToken(Users user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var signingKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(_configuration["JWTSettings:SecretKey"]));

            var getToken = new JwtSecurityToken
                (audience: _configuration["JWTSettings:Audience"],
                issuer: _configuration["JWTSettings:Issuer"],
                claims: authClaims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(getToken);
        }
    }
}
