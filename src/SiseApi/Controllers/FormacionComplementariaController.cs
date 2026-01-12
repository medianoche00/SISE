using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SiseApi.Data;
using SiseApi.Data.Models;
using SiseApi.Models;
using SiseApi.Services;

namespace SiseApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FormacionComplementariaController : Controller
{
    private readonly SiseDbContext _context;
    private readonly IUsuarioActualService _usuarioActualService;

    public FormacionComplementariaController(SiseDbContext context, IUsuarioActualService usuarioActualService)
    {
        _context = context;
        _usuarioActualService = usuarioActualService;
    }

    [HttpGet]
    public async Task<ActionResult<List<FormacionComplementariaDto>>> GetMisFormaciones()
    {
        var idEgresado = await _usuarioActualService.GetIdEgresadoActualAsync();
        if (idEgresado == null) return Unauthorized("Usuario no es egresado.");

        var formaciones = await _context.FormacionComplementaria
            .Where(f => f.IdEgresado == idEgresado && f.Estado != "Eliminado")
            .OrderByDescending(e => e.FechaInicio) // Orden cronológico inverso
            .Select(f => new FormacionComplementariaDto
            {
                IdFormacion = f.IdFormacion,
                TipoFormacion = f.IdTipoFormacionNavigation.NombreTipoFormacion,
                NombreDelCurso = f.NombreDelCurso,
                Institucion = f.Institucion,
                FechaInicio = f.FechaInicio,
                FechaFin = f.FechaFin,
                Estado = f.Estado
            }).ToListAsync();

        return Ok(formaciones);
    }

    [HttpPost]
    public async Task<IActionResult> CrearFormacion([FromBody] FormacionComplementariaDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        Console.WriteLine("no badrequest");

        var idEgresado = await _usuarioActualService.GetIdEgresadoActualAsync();
        if (idEgresado == null) return Unauthorized("No se encontró el perfil de egresado.");

        if (dto.IdTipoFormacion == null)
        {
            var tipoFormacionExistente = await _context.TipoFormacion
                .FirstOrDefaultAsync(t => t.NombreTipoFormacion.ToLower() == dto.TipoFormacion.ToLower());
            if (tipoFormacionExistente == null)
            {
                return BadRequest("El tipo de formación especificado no existe.");
            }
            dto.IdTipoFormacion = tipoFormacionExistente.IdTipoFormacion;
        }

        var nuevaExperiencia = new FormacionComplementaria
        {
            IdEgresado = idEgresado.Value,
            IdTipoFormacion = (int)dto.IdTipoFormacion,
            NombreDelCurso = dto.NombreDelCurso,
            Institucion = dto.Institucion,
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin,
            Estado = "Pendiente"
        };

        _context.FormacionComplementaria.Add(nuevaExperiencia);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Formacion agregada correctamente." });
    }

    [HttpPut]
    public async Task<IActionResult> EditarFormacion(int id, [FromBody] FormacionComplementariaDto dto)
    {
        var idEgresado = await _usuarioActualService.GetIdEgresadoActualAsync();
        if (idEgresado == null) return Unauthorized();

        var formacion = await _context.FormacionComplementaria
            .FirstOrDefaultAsync(e => e.IdFormacion == id && e.IdEgresado == idEgresado);

        if (formacion == null) return NotFound("Experiencia no encontrada o no te pertenece.");

        if (dto.IdTipoFormacion == null)
        {
            var tipoFormacionExistente = await _context.TipoFormacion
                .FirstOrDefaultAsync(t => t.NombreTipoFormacion.ToLower() == dto.TipoFormacion.ToLower());
            if (tipoFormacionExistente == null)
            {
                return BadRequest("El tipo de formación especificado no existe.");
            }
            dto.IdTipoFormacion = tipoFormacionExistente.IdTipoFormacion;
        }

        formacion.IdEgresado = idEgresado.Value;
        formacion.IdTipoFormacion = (int)dto.IdTipoFormacion;
        formacion.NombreDelCurso = dto.NombreDelCurso;
        formacion.Institucion = dto.Institucion;
        formacion.FechaInicio = dto.FechaInicio;
        formacion.FechaFin = dto.FechaFin;
        formacion.Estado = dto.Estado;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Experiencia actualizada." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarFormacion(int id)
    {
        var idEgresado = await _usuarioActualService.GetIdEgresadoActualAsync();
        if (idEgresado == null) return Unauthorized();

        var formacion = await _context.FormacionComplementaria
            .FirstOrDefaultAsync(e => e.IdFormacion == id && e.IdEgresado == idEgresado);

        if (formacion == null) return NotFound("Experiencia no encontrada.");

        formacion.Estado = "Eliminado";
        await _context.SaveChangesAsync();

        return Ok(new { message = "Experiencia eliminada." });
    }
}
