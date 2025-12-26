using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Data.Models;

namespace SiseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfertaLaboralController : ControllerBase
    {
        private readonly SiseDbContext _context;
        public OfertaLaboralController(SiseDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<OfertaLaboral>> Get()
        {
            return await _context.OfertasLaborales
                         .Where(x => x.Estado == true) //solo muestra ofertas que estan activas
                         .Include(x => x.IdEmpresaNavigation)
                         .Include(x => x.IdTipoContratoNavigation)
                         .Include(x => x.IdModalidadTrabajoNavigation)
                         .Include(x => x.IdEgresadoGanadorNavigation)
                         .ToListAsync();
        }

        [HttpGet("{id:int}", Name = "GetOfertaLaboral")]
        public async Task<ActionResult<OfertaLaboral>> Get(int id)
        {
            var result = await _context.OfertasLaborales.FirstOrDefaultAsync(x => x.IdOferta == id);
            if (result is null)
            {
                return NotFound();
            }
            return result;
        }

        [HttpPost]
        public async Task<ActionResult> Post(OfertaLaboral ofertaLaboral)
        {
            _context.Add(ofertaLaboral);
            await _context.SaveChangesAsync();
            return new CreatedAtRouteResult("GetOfertaLaboral", new { id = ofertaLaboral.IdOferta }, ofertaLaboral);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, OfertaLaboral ofertaLaboral)
        {
            if (await _context.OfertasLaborales.FindAsync(id) is null)
            {
                return NotFound();
            }
            _context.Update(ofertaLaboral);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var ofertaLaboral = await _context.OfertasLaborales.FindAsync(id);
            if (ofertaLaboral == null)
            {
                return NotFound();
            }
            ofertaLaboral.Estado = false;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
