using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SiseApi.Data.Models;
using SiseApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SiseApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _cfg;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenService(IConfiguration cfg, UserManager<ApplicationUser> userManager)
        {
            _cfg = cfg;
            _userManager = userManager;
        }

        public async Task<AuthResponse> CreateTokenAsync(ApplicationUser user)
        {
            var key = _cfg["Jwt:Key"];
            var issuer = _cfg["Jwt:Issuer"];
            var audience = _cfg["Jwt:Audience"];
            var minutes = int.Parse(_cfg["Jwt:TokenLifetimeMinutes"] ?? "60");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            };

            if (!string.IsNullOrEmpty(user.Email))
            {
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
            }

            // Añadir roles como claims
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var r in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, r));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(minutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponse
            {
                Token = tokenString,
                TokenType = "Bearer",
                ExpiresAt = expires,
                UserName = user.UserName,
                UserId = user.Id
            };
        }
    }
}
