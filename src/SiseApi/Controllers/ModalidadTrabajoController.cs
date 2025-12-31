using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Models;

namespace SiseApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ModalidadTrabajoController : ControllerBase
{
    public readonly SiseDbContext _context;
    public ModalidadTrabajoController(SiseDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<ModalidadTrabajoDto>>> GetModalidades()
    {
        var modalidades = await _context.ModalidadesTrabajos
            .Where(m => m.Estado == true) //solo modalidades activas
            .Select(m => new ModalidadTrabajoDto 
            {
                IdModalidad = m.IdModalidadTrabajo,
                NombreModalidad = m.NombreModalidad
            }).ToListAsync();

        return Ok(modalidades);
    }
}
