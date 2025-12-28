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
    public class EgresadosController : ControllerBase
    {
        private readonly SiseDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EgresadosController(SiseDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [HttpGet("mi-perfil-egresado")]
        public async Task<ActionResult<PerfilEgresadoDto>> GetMiPerfil()
        {
            // Obtener usuario logueado
            var userIdClaim = User.FindFirst("uid")?.Value
                           ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
            int userId = int.Parse(userIdClaim);

            // Buscar egresado con sus relaciones (Persona y Carrera)
            var egresado = await _context.Egresados
                .Include(e => e.Persona)
                .Include(e => e.Carrera)
                .FirstOrDefaultAsync(x => x.IdUsuario == userId);

            if (egresado == null) return Forbid("El usuario no es un egresado.");
            if (egresado.Persona == null) return NotFound("Datos personales no encontrados.");

            // Mapear a DTO para enviar al Front
            var perfilDto = new PerfilEgresadoDto
            {
                // Personales
                Nombres = egresado.Persona.Nombres,
                ApellidoPaterno = egresado.Persona.ApellidoPaterno,
                ApellidoMaterno = egresado.Persona.ApellidoMaterno,
                DocumentoIdentidad = egresado.Persona.DocumentoIdentidad, // O DNI

                // Contacto
                Telefono = egresado.Persona.Telefono,
                CorreoPersonal = egresado.Persona.CorreoPersonal,

                // Académicos
                IdCarrera = egresado.IdCarrera,
                CodigoUniversitario = egresado.CodigoUniversitario, // Asumiendo que existe en Egresado
                AñoEgreso = egresado.AñoEgreso, // Asumiendo que es int
                Carrera = egresado.Carrera.NombreCarrera
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

                var egresado = await _context.Egresados
                    .Include(e => e.Persona)
                    .FirstOrDefaultAsync(e => e.IdUsuario == userId);

                if (egresado == null)
                {
                    return BadRequest("No se encontró un perfil de egresado asociado a este usuario.");
                }

                if (egresado.Persona == null)
                {
                    return BadRequest("El egresado no tiene datos personales asociados.");
                }

                egresado.Persona.Nombres = dto.Nombres;
                egresado.Persona.ApellidoPaterno = dto.ApellidoPaterno;
                egresado.Persona.ApellidoPaterno = dto.ApellidoMaterno;
                //egresado.Persona.DocumentoIdentidad = dto.DocumentoIdentidad;
                egresado.Persona.Telefono = dto.Telefono;
                egresado.Persona.CorreoPersonal = dto.CorreoPersonal;
                egresado.AñoEgreso = dto.AñoEgreso;

                await _context.SaveChangesAsync();
                return Ok(new { message = "Perfil actualizado correctamente." });
            }
            catch (Exception ex)
            {
                var mensajeError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, new { message = "Error interno: " + mensajeError });
            }
        }

        [HttpPost("actualizar-experiencia")]
        public async Task<IActionResult> ActualizarExperiencia([FromBody] ExperienciaLaboralDto dto)
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

                var egresado = await _context.Egresados
                    .Include(e => e.ExperienciaLaborals)
                    .FirstOrDefaultAsync(e => e.IdUsuario == userId);

                if (egresado == null)
                {
                    return BadRequest("No se encontró un perfil de egresado asociado a este usuario.");
                }

                foreach (var expDto in dto)
                {
                    var experiencia = egresado.ExperienciaLaborals
                        .FirstOrDefault(el => el.IdExperienciaLaboral == expDto.IdExperienciaLaboral);
                    if (experiencia != null)
                    {
                        // Actualizar experiencia existente
                        experiencia.Empresa = expDto.Empresa;
                        experiencia.Cargo = expDto.Cargo;
                        experiencia.FechaInicio = expDto.FechaInicio;
                        experiencia.FechaFin = expDto.FechaFin;
                        experiencia.Actualmente = expDto.Actualmente;
                    }
                    else
                    {
                        // Agregar nueva experiencia
                        var nuevaExperiencia = new ExperienciaLaboral
                        {
                            IdEgresado = egresado.IdEgresado,
                            Empresa = expDto.Empresa,
                            Cargo = expDto.Cargo,
                            FechaInicio = expDto.FechaInicio,
                            FechaFin = expDto.FechaFin,
                            Actualmente = expDto.Actualmente
                        };
                        _context.ExperienciaLaborals.Add(nuevaExperiencia);
                    }
                }

                egresado.Persona.Telefono = dto.NumeroTelefono;
                egresado.Persona.CorreoPersonal = dto.CorreoPersonal;

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