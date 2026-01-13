using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Data.Models;
using SiseApi.Models;
using SiseApi.Services;

namespace SiseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EgresadosController : ControllerBase
    {
        private readonly SiseDbContext _context;
        private readonly IUsuarioActualService _usuarioActualService;

        public EgresadosController(SiseDbContext context, IUsuarioActualService usuarioActual)
        {
            _context = context;
            _usuarioActualService = usuarioActual;
        }


        [HttpGet("mi-perfil-egresado")]
        public async Task<ActionResult<PerfilEgresadoDto>> GetMiPerfil()
        {
            var idEgresado = await _usuarioActualService.GetIdEgresadoActualAsync();
            
            var egresado = await _context.Egresado
                .Include(e => e.IdPersonaNavigation)
                .Include(e => e.IdCarreraNavigation)
                .FirstOrDefaultAsync(x => x.IdEgresado == idEgresado);

            if (egresado == null) return Forbid("El usuario no es un egresado.");
            if (egresado.IdPersonaNavigation == null) return NotFound("Datos personales no encontrados.");

            // Mapear a DTO para enviar al Front
            var perfilDto = new PerfilEgresadoDto
            {
                // Personales
                Nombres = egresado.IdPersonaNavigation.Nombres,
                ApellidoPaterno = egresado.IdPersonaNavigation.ApellidoPaterno,
                ApellidoMaterno = egresado.IdPersonaNavigation.ApellidoMaterno,
                NumeroDocumento = egresado.IdPersonaNavigation.NumeroDocumento, // O DNI

                // Contacto
                Telefono = egresado.IdPersonaNavigation.Telefono,
                CorreoPersonal = egresado.IdPersonaNavigation.CorreoPersonal,

                // Académicos
                IdCarrera = egresado.IdCarrera,
                CodigoUniversitario = egresado.CodigoUniversitario, // Asumiendo que existe en Egresado
                AñoEgreso = egresado.AnioEgreso, // Asumiendo que es int
                Carrera = egresado.IdCarreraNavigation.NombreCarrera
            };

            return Ok(perfilDto);
        }


        [HttpPost("actualizar-perfil")]
        public async Task<IActionResult> ActualizarPerfil([FromBody] PerfilEgresadoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Obtener usuario logueado
                var userIdClaim = User.FindFirst("uid")?.Value
                               ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
                int userId = int.Parse(userIdClaim);

                var egresado = await _context.Egresado
                    .Include(e => e.IdPersonaNavigation)
                    .FirstOrDefaultAsync(e => e.IdUsuario == userId);

                if (egresado == null)
                {
                    return BadRequest("No se encontró un perfil de egresado asociado a este usuario.");
                }

                if (egresado.IdPersonaNavigation == null)
                {
                    return BadRequest("El egresado no tiene datos personales asociados.");
                }

                egresado.IdPersonaNavigation.Nombres = dto.Nombres;
                egresado.IdPersonaNavigation.ApellidoPaterno = dto.ApellidoPaterno;
                egresado.IdPersonaNavigation.ApellidoPaterno = dto.ApellidoMaterno;
                //egresado.Persona.NumeroDocumento = dto.NumeroDocumento;
                egresado.IdPersonaNavigation.Telefono = dto.Telefono;
                egresado.IdPersonaNavigation.CorreoPersonal = dto.CorreoPersonal;
                egresado.AnioEgreso = dto.AñoEgreso;

                await _context.SaveChangesAsync();
                return Ok(new { message = "Perfil actualizado correctamente." });
            }
            catch (Exception ex)
            {
                var mensajeError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, new { message = "Error interno: " + mensajeError });
            }
        }
    }
}