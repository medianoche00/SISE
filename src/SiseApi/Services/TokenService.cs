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
        private readonly UserManager<IdentityUser<int>> _userManager;

        public TokenService(IConfiguration cfg, UserManager<IdentityUser<int>> userManager)
        {
            _cfg = cfg;
            _userManager = userManager;
        }

        public async Task<AuthResponse> CreateTokenAsync(IdentityUser<int> user)
        {
            // CORRECCIÓN AQUÍ: Agregamos el ?? "..." para que nunca sea nulo
            var key = _cfg["Jwt:Key"] ?? "UnaClaveSuperSecretaYLoSuficientementeLarga123456";

            var issuer = _cfg["Jwt:Issuer"] ?? "SiseApi"; // También protegemos estos por si acaso
            var audience = _cfg["Jwt:Audience"] ?? "SiseApiUsers";
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

            // AHORA SÍ: "key" tiene valor seguro, así que GetBytes ya no se queja
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
                UserId = user.Id,
                Role = roles.FirstOrDefault() ?? string.Empty
            };
        }
    }
}
