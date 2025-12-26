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

        /*--------------------- Otros -----------------------*/

        // GET: api/OfertaLaboral/empresa/5
        [HttpGet("empresa/{idEmpresa:int}")]
        public async Task<ActionResult<IEnumerable<OfertaLaboral>>> GetByEmpresa(int idEmpresa)
        {
            //deberia usar procedimiento almacenado
            var ofertas = await _context.OfertasLaborales
                .Where(o => o.Estado == true && o.IdEmpresa == idEmpresa)
                .Include(o => o.IdModalidadTrabajoNavigation)
                .Include(o => o.IdTipoContratoNavigation)
                .AsNoTracking()
                .ToListAsync();

            if (ofertas == null || !ofertas.Any())
            {
                return NotFound("No se encontraron ofertas para esta empresa.");
            }

            return ofertas;
        }

        // GET: api/OfertaLaboral/mis-postulaciones/10
        [HttpGet("mis-postulaciones/{idEgresado:int}")]
        public async Task<ActionResult<IEnumerable<OfertaLaboral>>> GetByEgresado(int idEgresado)
        {
            // Opción A: Si tienes un DbSet<Postulacion>
            // Buscamos en la tabla intermedia y "saltamos" hacia la oferta
            var ofertasPostuladas = await _context.Postulaciones
                .Where(p => p.IdEgresado == idEgresado) // Filtro por usuario
                .Include(p => p.OfertaLaboral)          // Cargo la oferta
                    .ThenInclude(o => o.IdEmpresaNavigation) // Cargar empresa de la oferta (opcional)
                .Select(p => p.OfertaLaboral)           // Selecciono solo el objeto Oferta para devolver
                .Where(o => o.Estado == true) // Asegurarse de que la oferta esté activa
                .AsNoTracking()
                .ToListAsync();

            if (!ofertasPostuladas.Any())
            {
                return NotFound("El usuario no ha postulado a ninguna oferta.");
            }

            return ofertasPostuladas;
        }

        // GET: api/OfertaLaboral/activas
        [HttpGet("activas")]
        public async Task<List<OfertaLaboral>> GetActivas()
        {
            return await _context.OfertasLaborales
                         .Where(x => x.Estado == true //solo muestra ofertas que estan activas
                            && x.FechaCierre >= DateOnly.FromDateTime(DateTime.Now)) //no pasan de su fecha de cierre
                         .Include(x => x.IdEmpresaNavigation)
                         .Include(x => x.IdTipoContratoNavigation)
                         .Include(x => x.IdModalidadTrabajoNavigation)
                         .Include(x => x.IdEgresadoGanadorNavigation)
                         .ToListAsync();
        }

        //metodo para postular

    }
}
