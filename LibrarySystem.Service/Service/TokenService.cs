using LibrarySystem.Core.Entitties.Identity;
using LibrarySystem.Core.Services.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Service.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser appUser, UserManager<AppUser> userManager)
        {
            var AuthClaims = new List<Claim>()
           {
               new Claim(ClaimTypes.GivenName, appUser.DisplayName),
               new Claim(ClaimTypes.Email , appUser.Email)
           };
            var Roles =await userManager.GetRolesAsync(appUser);
            foreach (var role in Roles)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:KEY"]));

            var Token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:Expiration"])),
                claims: AuthClaims,
                signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256Signature)
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(Token);

            return jwt;
        }
    }
}
