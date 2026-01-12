using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Data.Models;
using SiseApi.Models;
using SiseApi.Services;

namespace SiseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfertaLaboralController : ControllerBase
    {
        private readonly SiseDbContext _context;
        private readonly IUsuarioActualService _usuarioActualService;

        public OfertaLaboralController(SiseDbContext context, IUsuarioActualService usuarioActualService)
        {
            _context = context;
            _usuarioActualService = usuarioActualService;
        }

        // ==========================================
        // SECCIÓN EGRESADO
        // ==========================================

        // Ver las ofertas disponibles (Filtro: Activas y Fecha vigente)
        [HttpGet("disponibles")]
        public async Task<ActionResult<List<OfertaLaboralDto>>> GetDisponibles()
        {
            var ofertas = await _context.OfertaLaboral
                .Where(x => x.Estado != "Eliminado" && x.FechaCierre >= DateOnly.FromDateTime(DateTime.Now))
                .Include(x => x.IdEmpresaNavigation)
                .Include(x => x.IdTipoContratoNavigation)
                .Include(x => x.IdModalidadTrabajoNavigation)
                .AsNoTracking()
                .Select(x => new OfertaLaboralDto
                {
                    IdOferta = x.IdOferta,
                    Titulo = x.Titulo,
                    Descripcion = x.Descripcion,
                    Requisitos = x.Requisitos,
                    Ubicacion = x.Ubicacion,
                    Sueldo = (decimal)x.Sueldo,
                    FechaCierre = x.FechaCierre,
                    TipoContrato = x.IdTipoContratoNavigation.NombreTipo,
                    Modalidad = x.IdModalidadTrabajoNavigation.NombreModalidad,

                    EmpresaRuc = x.IdEmpresaNavigation.Ruc,
                    EmpresaRazonSocial = x.IdEmpresaNavigation.RazonSocial,
                    EmpresaTelefono = x.IdEmpresaNavigation.Telefono,
                    EmpresaCorreo = x.IdEmpresaNavigation.Correo
                })
                .ToListAsync();

            return Ok(ofertas);
        }

        // ==========================================
        // SECCIÓN REPRESENTANTE
        // ==========================================

        // Ver las ofertas de MI empresa
        [HttpGet("mis-ofertas")]
        public async Task<ActionResult<IEnumerable<OfertaLaboral>>> GetMisOfertas()
        {
            var idRepresentante = await _usuarioActualService.GetIdRepresentanteActualAsync();
            if (idRepresentante == null) return Unauthorized("Usuario no es representante.");

            var representante = await _context.Representante.FindAsync(idRepresentante);

            var ofertas = await _context.OfertaLaboral
                .Where(o => o.IdEmpresa == representante.IdEmpresa) // Muestra todas (activas e inactivas según tu lógica, o filtra aquí)
                .Include(o => o.IdModalidadTrabajoNavigation)
                .Include(o => o.IdTipoContratoNavigation)
                .AsNoTracking()
                .ToListAsync();

            return Ok(ofertas);
        }

        // Agregar oferta
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CrearOfertaDto ofertaDto)
        {
            var idRepresentante = await _usuarioActualService.GetIdRepresentanteActualAsync();
            if (idRepresentante == null) return Unauthorized("Usuario no es representante.");

            var representante = await _context.Representante.FindAsync(idRepresentante);

            var nuevaOferta = new OfertaLaboral
            {
                IdEmpresa = representante.IdEmpresa,
                Titulo = ofertaDto.Titulo,
                Descripcion = ofertaDto.Descripcion,
                Requisitos = ofertaDto.Requisitos,
                Ubicacion = ofertaDto.Ubicacion,
                Sueldo = ofertaDto.Sueldo,
                IdTipoContrato = ofertaDto.IdTipoContrato,
                IdModalidadTrabajo = ofertaDto.IdModalidadTrabajo,
                FechaPublicacion = DateOnly.FromDateTime(DateTime.Now),
                FechaCierre = ofertaDto.FechaCierre,
                Estado = "Activo" // Se crea activa por defecto
            };

            _context.OfertaLaboral.Add(nuevaOferta);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Oferta creada exitosamente", id = nuevaOferta.IdOferta });
        }

        // Eliminar oferta (Soft Delete: cambia estado a false)
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var idRepresentante = await _usuarioActualService.GetIdRepresentanteActualAsync();
            if (idRepresentante == null) return Unauthorized("Usuario no es representante.");

            var representante = await _context.Representante.FindAsync(idRepresentante);
            var oferta = await _context.OfertaLaboral.FindAsync(id);

            if (oferta == null) return NotFound();

            // Verificar que la oferta pertenece a la empresa del representante
            if (oferta.IdEmpresa != representante.IdEmpresa)
                return Forbid("No tienes permiso para eliminar esta oferta.");

            if (oferta.Estado == "Eliminado")
                return BadRequest("La oferta ya está eliminada.");

            oferta.Estado = "Eliminado";
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
