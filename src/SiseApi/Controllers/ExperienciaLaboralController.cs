using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Data.Models;
using SiseApi.Models;
using SiseApi.Services;

namespace SiseApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExperienciaLaboralController : ControllerBase
{
    private readonly SiseDbContext _context;
    private readonly IUsuarioActualService _usuarioActualService;

    public ExperienciaLaboralController(SiseDbContext context, IUsuarioActualService usuarioActualService)
    {
        _context = context;
        _usuarioActualService = usuarioActualService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ExperienciaLaboralDto>>> GetMisExperiencias()
    {
        var idEgresado = await _usuarioActualService.GetIdEgresadoActualAsync();
        if (idEgresado == null) return Unauthorized("Usuario no es egresado.");

        var experiencias = await _context.ExperienciaLaboral
            .Where(e => e.IdEgresado == idEgresado && e.Estado != "Eliminado")
            .OrderByDescending(e => e.FechaInicio) // Orden cronológico inverso
            .Select(e => new ExperienciaLaboralDto
            {
                IdExperiencia = e.IdExperiencia,
                Empresa = e.Empresa,
                Cargo = e.Cargo,
                FechaInicio = e.FechaInicio,
                FechaFin = e.FechaFin,
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
        Console.WriteLine("no badrequest");

        var idEgresado = await _usuarioActualService.GetIdEgresadoActualAsync();
        if (idEgresado == null) return Unauthorized("No se encontró el perfil de egresado.");

        var nuevaExperiencia = new ExperienciaLaboral
        {
            IdEgresado = idEgresado.Value,
            Empresa = dto.Empresa,
            Cargo = dto.Cargo,
            Descripcion = dto.Descripcion,
            Estado = "Buscando Trabajo",
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.Actualmente ? null : dto.FechaFin
        };

        _context.ExperienciaLaboral.Add(nuevaExperiencia);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Experiencia agregada correctamente." });
    }

    [HttpPut]
    public async Task<IActionResult> EditarExperiencia(int id, [FromBody] ExperienciaLaboralDto dto)
    {
        var idEgresado = await _usuarioActualService.GetIdEgresadoActualAsync();
        if (idEgresado == null) return Unauthorized();

        var experiencia = await _context.ExperienciaLaboral
            .FirstOrDefaultAsync(e => e.IdExperiencia == id && e.IdEgresado == idEgresado);

        if (experiencia == null) return NotFound("Experiencia no encontrada o no te pertenece.");

        experiencia.Empresa = dto.Empresa;
        experiencia.Cargo = dto.Cargo;
        experiencia.Descripcion = dto.Descripcion;
        experiencia.FechaInicio = dto.FechaInicio;
        experiencia.FechaFin = dto.FechaFin;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Experiencia actualizada." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarExperiencia(int id)
    {
        var idEgresado = await _usuarioActualService.GetIdEgresadoActualAsync();
        if (idEgresado == null) return Unauthorized();

        var experiencia = await _context.ExperienciaLaboral
            .FirstOrDefaultAsync(e => e.IdExperiencia == id && e.IdEgresado == idEgresado);

        if (experiencia == null) return NotFound("Experiencia no encontrada.");

        experiencia.Estado = "Eliminado";
        await _context.SaveChangesAsync();

        return Ok(new { message = "Experiencia eliminada." });
    }
}