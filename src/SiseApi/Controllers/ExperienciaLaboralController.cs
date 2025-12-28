using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Data.Models;
using SiseApi.Models;

namespace SiseApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExperienciasController : ControllerBase
{
    private readonly SiseDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ExperienciasController(SiseDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // HELPER: Obtener el ID del Egresado basado en el usuario logueado
    private async Task<int?> ObtenerIdEgresadoActual()
    {
        var userIdClaim = User.FindFirst("uid")?.Value
                       ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim)) return null;
        int userId = int.Parse(userIdClaim);

        var egresado = await _context.Egresados
            .Select(e => new { e.IdEgresado, e.IdUsuario })
            .FirstOrDefaultAsync(e => e.IdUsuario == userId);

        return egresado?.IdEgresado;
    }

    [HttpGet]
    public async Task<ActionResult<List<ExperienciaLaboralDto>>> GetMisExperiencias()
    {
        var idEgresado = await ObtenerIdEgresadoActual();
        if (idEgresado == null) return Unauthorized("Usuario no es egresado.");

        var experiencias = await _context.ExperienciasLaborales
            .Where(e => e.IdEgresado == idEgresado && e.Estado == true)
            .OrderByDescending(e => e.FechaInicio) // Orden cronológico inverso
            .Select(e => new ExperienciaLaboralDto
            {
                IdExperiencia = e.IdExperiencia,
                Empresa = e.Empresa,
                Cargo = e.Cargo,
                FechaInicio = e.FechaInicio.ToDateTime(TimeOnly.MinValue),
                FechaFin = e.FechaFin.HasValue ? e.FechaFin.Value.ToDateTime(TimeOnly.MinValue) : null,
                Actualmente = e.FechaFin == null,
                Descripcion = e.Descripcion
            })
            .ToListAsync();

        return Ok(experiencias);
    }

    [HttpPost]
    public async Task<IActionResult> CrearExperiencia([FromBody] ExperienciaLaboralDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var idEgresado = await ObtenerIdEgresadoActual();
        if (idEgresado == null) return Unauthorized("No se encontró el perfil de egresado.");

        var nuevaExperiencia = new ExperienciaLaboral
        {
            IdEgresado = idEgresado.Value,
            Empresa = dto.Empresa,
            Cargo = dto.Cargo,
            Descripcion = dto.Descripcion,
            Estado = true,
            FechaInicio = DateOnly.FromDateTime(dto.FechaInicio),
            FechaFin = dto.Actualmente ? null : (dto.FechaFin.HasValue ? DateOnly.FromDateTime(dto.FechaFin.Value) : null)
        };

        _context.ExperienciasLaborales.Add(nuevaExperiencia);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Experiencia agregada correctamente." });
    }

    [HttpPut]
    public async Task<IActionResult> EditarExperiencia(int id, [FromBody] ExperienciaLaboralDto dto)
    {
        var idEgresado = await ObtenerIdEgresadoActual();
        if (idEgresado == null) return Unauthorized();

        // Buscamos la experiencia y validamos que pertenezca al usuario actual (Seguridad)
        var experiencia = await _context.ExperienciasLaborales
            .FirstOrDefaultAsync(e => e.IdExperiencia == id && e.IdEgresado == idEgresado);

        if (experiencia == null) return NotFound("Experiencia no encontrada o no te pertenece.");

        // Actualizamos campos
        experiencia.Empresa = dto.Empresa;
        experiencia.Cargo = dto.Cargo;
        experiencia.Descripcion = dto.Descripcion;
        experiencia.FechaInicio = DateOnly.FromDateTime(dto.FechaInicio);
        experiencia.FechaFin = dto.Actualmente ? null : (dto.FechaFin.HasValue ? DateOnly.FromDateTime(dto.FechaFin.Value) : null);

        await _context.SaveChangesAsync();

        return Ok(new { message = "Experiencia actualizada." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarExperiencia(int id)
    {
        var idEgresado = await ObtenerIdEgresadoActual();
        if (idEgresado == null) return Unauthorized();

        var experiencia = await _context.ExperienciasLaborales
            .FirstOrDefaultAsync(e => e.IdExperiencia == id && e.IdEgresado == idEgresado);

        if (experiencia == null) return NotFound("Experiencia no encontrada.");

        experiencia.Estado = false; 

        await _context.SaveChangesAsync();

        return Ok(new { message = "Experiencia eliminada." });
    }
}