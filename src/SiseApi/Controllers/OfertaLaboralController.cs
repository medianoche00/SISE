using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Data.Models;
using SiseApi.Models;

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
        public async Task<ActionResult<IEnumerable<OfertaLaboral>>> GetByEgresado(int idUsuario)
        {
            var egresado = await _context.Egresados
                .FirstOrDefaultAsync(x => x.IdUsuario == idUsuario);

            if (egresado == null)
            {
                return BadRequest("El usuario no existe o no es un egresado.");
            }

            var ofertasPostuladas = await _context.Postulaciones
                .Where(p => p.IdEgresado == egresado.IdEgresado) // Filtro por usuario
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

        // POST: api/OfertaLaboral/postular
        [HttpPost("postular")]
        public async Task<ActionResult> Postular([FromBody] PostulacionRequest request)
        {
            // Validar que la oferta exista y esté activa
            var oferta = await _context.OfertasLaborales
                .FirstOrDefaultAsync(x => x.IdOferta == request.IdOferta && x.Estado == true);

            if (oferta == null)
            {
                return BadRequest("La oferta no existe o no está activa.");
            }

            // Validar que el usuario exista y sea un egresado
            var egresado = await _context.Egresados
                .FirstOrDefaultAsync(x => x.IdUsuario == request.IdUsuario);

            if (egresado == null)
            { 
                return BadRequest("El usuario no existe o no es un egresado.");
            }

            // Validar que la fecha de cierre no haya pasado
            // Convertimos DateOnly a DateTime para comparar o usamos DateOnly.FromDateTime
            if (oferta.FechaCierre < DateOnly.FromDateTime(DateTime.Now))
            {
                return BadRequest("La oferta ha finalizado, ya no se aceptan postulaciones.");
            }

            // Validar si el usuario YA se postuló previamente
            bool yaPostulado = await _context.Postulaciones
                .Where(p => p.Estado != "CANCELADO") // no se cuentan postulaciones canceladas
                .AnyAsync(p => p.IdOferta == request.IdOferta && p.IdEgresado == egresado.IdEgresado);

            if (yaPostulado)
            {
                return BadRequest("Ya te has postulado a esta oferta anteriormente.");
            }

            // Crear la entidad Postulacion
            var nuevaPostulacion = new Postulacion
            {
                IdOferta = request.IdOferta,
                IdEgresado = egresado.IdUsuario,
                FechaPostulacion = DateTime.Now,
                Estado = "Pendiente", // Estado inicial
                IdRepresentanteEvaluador = null,
                FechaEvaluacion = null,
                Comentarios = null
            };

            try
            {
                _context.Postulaciones.Add(nuevaPostulacion);
                await _context.SaveChangesAsync();

                // Retornamos Ok con un mensaje o el objeto creado
                return Ok(new { message = "Postulación realizada con éxito", idPostulacion = nuevaPostulacion.IdPostulacion });
            }
            catch (DbUpdateException)
            {
                // En caso de que falle la concurrencia y se intente duplicar al mismo milisegundo
                return StatusCode(500, "Error al procesar la postulación.");
            }
        }

        [HttpDelete("cancelar-postulacion/{idOferta:int}/{idUsuario:int}")]
        public async Task<ActionResult> CancelarPostulacion(int idOferta, int idUsuario)
        {
            var egresado = await _context.Egresados
                .FirstOrDefaultAsync(e => e.IdUsuario == idUsuario);

            if (egresado == null)
            {
                return NotFound("No se encontró al egresado.");
            }

            var postulacion = await _context.Postulaciones
                .FirstOrDefaultAsync(p => p.IdOferta == idOferta && p.IdEgresado == egresado.IdEgresado);

            if (postulacion == null)
            {
                return NotFound("No se encontró la postulación.");
            }

            postulacion.Estado = "CANCELADO";
            _context.Postulaciones.Update(postulacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }

    public class PostulacionRequest
    {
        public int IdOferta { get; set; }
        public int IdUsuario { get; set; }
    }
}
