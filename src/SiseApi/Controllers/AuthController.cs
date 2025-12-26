using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiseApi.Data.Models;
using SiseApi.Models;
using SiseApi.Services;

namespace SiseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Buscar usuario
            ApplicationUser? user = await _userManager.FindByEmailAsync(dto.UserNameOrEmail)
                                    ?? await _userManager.FindByNameAsync(dto.UserNameOrEmail);

            if (user == null) return Unauthorized(new { message = "Usuario o contraseña incorrectos." });

            // Verificar contraseña
            var valid = await _userManager.CheckPasswordAsync(user, dto.Password);
            
            if (!valid) return Unauthorized(new { message = "Usuario o contraseña incorrectos." });

            var token = await _tokenService.CreateTokenAsync(user);

            // 4. Devolver respuesta con Token y Roles
            return Ok(token);
        }
    }
}
