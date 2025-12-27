using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Data.Models;
using SiseApi.Models.ViewModels; 

namespace SiseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
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

        [HttpPost("completar-perfil")]
        public async Task<IActionResult> CompletarPerfil([FromBody] RegistroEgresadoViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var usuarioActual = await _userManager.GetUserAsync(User);
                if (usuarioActual == null) return Unauthorized("Usuario no encontrado.");

                var nuevaPersona = new Persona
                {
                    Nombres = modelo.Nombres,
                    ApellidoPaterno = modelo.ApellidoPaterno,
                    ApellidoMaterno = modelo.ApellidoMaterno,
                    DocumentoIdentidad = modelo.DocumentoIdentidad,
                    Telefono = modelo.Telefono,
                    CorreoPersonal = modelo.CorreoPersonal,
                    Estado = true
                };

                _context.Personas.Add(nuevaPersona);
                await _context.SaveChangesAsync();

                var nuevoEgresado = new Egresado
                {
                    IdPersona = nuevaPersona.IdPersona,
                    IdUsuario = usuarioActual.Id,
                    IdCarrera = modelo.IdCarrera,
                    CodigoUniversitario = modelo.CodigoUniversitario,
                    AñoEgreso = modelo.AñoEgreso,
                    Estado = true
                };

                _context.Egresados.Add(nuevoEgresado);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new { message = "Perfil completado exitosamente" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = "Error interno: " + ex.Message });
            }
        }
    }
}