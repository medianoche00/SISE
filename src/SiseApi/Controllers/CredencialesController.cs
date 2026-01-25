using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiseApi.Data;
using SiseApi.Models;
using SiseApi.Services;

namespace SiseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CredencialesController : ControllerBase
    {
        private readonly SiseDbContext _context;
        private readonly IUsuarioActualService _usuarioActualService;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public CredencialesController(SiseDbContext context, IUsuarioActualService usuarioActualService, UserManager<IdentityUser<int>> userManager)
        {
            _context = context;
            _usuarioActualService = usuarioActualService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult> ActualizarCredenciales([FromBody] CredencialesDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DocumentoRespaldo))
            {
                return BadRequest("Es obligatorio indicar el documento que respalda la operacion.");
            }

            var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            var user = await _userManager.FindByIdAsync(dto.IdUsuario.ToString());
            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // 2. Actualizar Username (solo si cambió)
            if (user.UserName != dto.Username)
            {
                // Identity valida automáticamente si el nombre ya existe
                var resultUsername = await _userManager.SetUserNameAsync(user, dto.Username);

                if (!resultUsername.Succeeded)
                {
                    return BadRequest(resultUsername.Errors);
                }
            }

            // 3. Actualizar Contraseña (Reseteo administrativo)
            if (!string.IsNullOrEmpty(dto.Password))
            {
                // A: Eliminamos la contraseña actual (Hash)
                var removeResult = await _userManager.RemovePasswordAsync(user);
                if (!removeResult.Succeeded)
                {
                    return BadRequest(removeResult.Errors);
                }

                // B: Agregamos la nueva contraseña (Identity la hashea y saltea automáticamente)
                var addResult = await _userManager.AddPasswordAsync(user, dto.Password);
                if (!addResult.Succeeded)
                {
                    return BadRequest(addResult.Errors); // Ej: "La contraseña requiere un número..."
                }
            }
            return Ok();
        }
    }
}
