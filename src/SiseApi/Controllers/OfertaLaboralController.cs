using Azure.Core;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        public OfertaLaboralController(SiseDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
            var usuarioActual = await _userManager.GetUserAsync(User);
            if (usuarioActual == null) return Unauthorized();

            var representante = await _context.Representantes
                .FirstOrDefaultAsync(r => r.IdUsuario == usuarioActual.Id);
            if (representante == null) return Forbid("Solo los representantes pueden crear ofertas.");

            // FORZAR el ID de la empresa
            ofertaLaboral.IdEmpresa = representante.IdEmpresa;

            // Asegurar estado inicial
            ofertaLaboral.Estado = true;

            _context.Add(ofertaLaboral);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetOfertaLaboral", new { id = ofertaLaboral.IdOferta }, ofertaLaboral);
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
            var usuarioActual = await _userManager.GetUserAsync(User);
            if (usuarioActual == null) return Unauthorized();

            var representante = await _context.Representantes
                .FirstOrDefaultAsync(r => r.IdUsuario == usuarioActual.Id);
            if (representante == null) return Forbid("Solo los representantes pueden eliminar ofertas.");

            // Buscar la oferta
            var ofertaLaboral = await _context.OfertasLaborales.FindAsync(id);
            if (ofertaLaboral == null) return NotFound();

            // VERIFICACIÓN DE PROPIEDAD
            if (ofertaLaboral.IdEmpresa != representante.IdEmpresa)
            {
                return Forbid("No tienes permiso para eliminar esta oferta.");
            }

            ofertaLaboral.Estado = false;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /*--------------------- Otros -----------------------*/

        // GET: api/OfertaLaboral/ofertas-empresa/5
        [HttpGet("ofertas-empresa/{idEpresa:int}")]
        public async Task<ActionResult<IEnumerable<OfertaLaboral>>> GetMisPublicaciones(int idEmpresa)
        {
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

        // GET: api/OfertaLaboral/mis-postulaciones
        [HttpGet("mis-postulaciones")]
        public async Task<ActionResult<IEnumerable<OfertaLaboral>>> GetMisPostulaciones()
        {
            var usuarioActual = await _userManager.GetUserAsync(User);
            if (usuarioActual == null) return Unauthorized();

            var egresado = await _context.Egresados
                .FirstOrDefaultAsync(x => x.IdUsuario == usuarioActual.Id);

            if (egresado == null) return BadRequest("No eres un egresado.");

            var ofertasPostuladas = await _context.Postulaciones
                .Where(p => p.IdEgresado == egresado.IdEgresado)
                .Include(p => p.OfertaLaboral)
                .Select(p => p.OfertaLaboral)
                .Where(o => o.Estado == true) //solo ofertas activas
                .AsNoTracking()
                .ToListAsync();

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
            //verificar usuario
            var usuarioActual = await _userManager.GetUserAsync(User);
            if (usuarioActual == null)
            {
                return Unauthorized("Debes iniciar sesión para realizar esta acción.");
            }

            //verificar egresado
            var egresado = await _context.Egresados
                .FirstOrDefaultAsync(e => e.IdUsuario == usuarioActual.Id);
            if (egresado == null)
            {
                return NotFound("El usuario actual no tiene un perfil de egresado.");
            }

            var oferta = await _context.OfertasLaborales
                .FirstOrDefaultAsync(x => x.IdOferta == request.IdOferta && x.Estado == true);
            
            //verificar oferta
            if (oferta == null || oferta.Estado == false)
            {
                return BadRequest("La oferta no existe o no está activa.");
            }

            // Validar que la fecha de cierre no haya pasado
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
                IdEgresado = egresado.IdEgresado,
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

                return CreatedAtAction(nameof(Postular), new { id = nuevaPostulacion.IdPostulacion }, new { message = "Éxito" });
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Error al procesar la postulación.");
            }
        }

        // DELETE: api/OfertaLaboral/cancelar-postulacion/5
        [HttpDelete("cancelar-postulacion/{idOferta:int}")]
        public async Task<ActionResult> CancelarPostulacion(int idOferta)
        {
            //verificar usuario
            var usuarioActual = await _userManager.GetUserAsync(User);
            if (usuarioActual == null)
            {
                return Unauthorized("Debes iniciar sesión para realizar esta acción.");
            }

            //verificar egresado
            var egresado = await _context.Egresados
                .FirstOrDefaultAsync(e => e.IdUsuario == usuarioActual.Id);
            if (egresado == null)
            {
                return NotFound("El usuario actual no tiene un perfil de egresado.");
            }

            //validar postulacion
            var postulacion = await _context.Postulaciones
                .FirstOrDefaultAsync(p => p.IdOferta == idOferta && p.IdEgresado == egresado.IdEgresado);
            if (postulacion == null)
            {
                return NotFound("No se encontró una postulación activa para esta oferta.");
            }

            if (postulacion.Estado == "CANCELADO")
            {
                return BadRequest("La postulación ya fue cancelada anteriormente.");
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
    }
}
