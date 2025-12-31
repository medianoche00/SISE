using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Models;

namespace SiseApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TipoFormacionController : ControllerBase
{
    public readonly SiseDbContext _context;
    public TipoFormacionController(SiseDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<TipoFormacionDto>>> GetModalidades()
    {
        var modalidades = await _context.TiposFormacions
            .Where(m => m.Estado == true) //solo tipos activos
            .Select(m => new TipoFormacionDto 
            {
                IdTipoFormacion = m.IdTipoFormacion,
                NombreTipoFormacion = m.NombreTipoFormacion
            }).ToListAsync();

        return Ok(modalidades);
    }
}
