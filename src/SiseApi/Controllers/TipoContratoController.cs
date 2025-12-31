using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Models;

namespace SiseApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TipoContratoController : ControllerBase
{
    public readonly SiseDbContext _context;
    public TipoContratoController(SiseDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<TipoContratoDto>>> GetModalidades()
    {
        var modalidades = await _context.TiposContratos
            .Where(m => m.Estado == true) //solo tipos activos
            .Select(m => new TipoContratoDto 
            {
                IdTipoContrato = m.IdTipoContrato,
                NombreTipo = m.NombreTipo
            }).ToListAsync();

        return Ok(modalidades);
    }
}
