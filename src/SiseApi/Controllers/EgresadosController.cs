using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Data.Models;
using SiseApi.Models;

namespace SiseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EgresadosController : ControllerBase
    {
        private readonly SiseDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EgresadosController(SiseDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("carreras")]
        public async Task<IActionResult> GetCarreras()
        {
            var carreras = await _context.Carreras
                .Where(c => c.Estado)
                .Select(c => new { c.IdCarrera, c.NombreCarrera })
                .ToListAsync();

            return Ok(carreras);
        }

        [HttpPost("actualizar-perfil")]
        public async Task<IActionResult> ActualizarPerfil([FromBody] ActualizarPerfilDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userIdClaim = User.FindFirst("uid")?.Value
                               ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("No se pudo identificar al usuario.");

                int userId = int.Parse(userIdClaim);

                var egresado = await _context.Egresados
                    .Include(e => e.Persona)
                    .FirstOrDefaultAsync(e => e.IdUsuario == userId);

                if (egresado == null)
                {
                    return BadRequest("Error: No se encontró un perfil de egresado asociado a este usuario.");
                }

                if (egresado.Persona == null)
                {
                    return BadRequest("Error de integridad: El egresado no tiene datos personales asociados.");
                }

                egresado.Persona.Telefono = dto.Telefono;
                egresado.Persona.CorreoPersonal = dto.CorreoPersonal;

                if (dto.ExperienciaLaboral != null && dto.ExperienciaLaboral.Count > 0)
                {
                    foreach (var expDto in dto.ExperienciaLaboral)
                    {
                        var nuevaExperiencia = new ExperienciaLaboral
                        {
                            IdEgresado = egresado.IdEgresado,

                            Empresa = expDto.Empresa,
                            Cargo = expDto.Cargo,

                            FechaInicio = DateOnly.FromDateTime(expDto.FechaInicio),

                            FechaFin = expDto.Actualmente
                                        ? null
                                        : (expDto.FechaFin.HasValue ? DateOnly.FromDateTime(expDto.FechaFin.Value) : null),

                            Estado = true,
                            Descripcion = "Registrado vía web"
                        };

                        _context.ExperienciasLaborales.Add(nuevaExperiencia);
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Perfil actualizado y experiencia agregada correctamente." });
            }
            catch (Exception ex)
            {
                var mensajeError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, new { message = "Error interno: " + mensajeError });
            }
        }
    }
}