using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Services;
using SiseApi.Models;

namespace SiseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostulacionesController : Controller
    {
        private readonly SiseDbContext _context;
        private readonly IUsuarioActualService _usuarioActualService;

        public PostulacionesController(SiseDbContext context, IUsuarioActualService usuarioActualService)
        {
            _context = context;
            _usuarioActualService = usuarioActualService;
        }

        // ==========================================
        // SECCIÓN EGRESADO
        // ==========================================

        [HttpGet("mis-postulaciones")]
        public async Task<ActionResult<List<Postulacion>>> GetMisPostulaciones()
        {
            var idEgresado = await _usuarioActualService.GetIdEgresadoActualAsync();
            if (idEgresado == null) return Unauthorized("Usuario no es egresado.");

            var misPostulaciones = await _context.Postulaciones
                .Where(p => p.IdEgresado == idEgresado)
                .Select(p => new MisPostulacionesDto
                {
                    IdPostulacion = p.IdPostulacion,
                    IdRepresentanteEvaluador = p.IdRepresentanteEvaluador,
                    FechaPostulacion = p.FechaPostulacion,
                    FechaEvaluacion = p.FechaEvaluacion,
                    Estado = p.Estado,
                    Comentarios = p.Comentarios,
                    CartaPresentacion = p.CartaPresentacion,

                    Oferta = new OfertaLaboralDto
                    {
                        IdOferta = p.OfertaLaboral.IdOferta,
                        Titulo = p.OfertaLaboral.Titulo,
                        Descripcion = p.OfertaLaboral.Descripcion,
                        Requisitos = p.OfertaLaboral.Requisitos,
                        Ubicacion = p.OfertaLaboral.Ubicacion,
                        Sueldo = (decimal)p.OfertaLaboral.Sueldo,
                        FechaCierre = p.OfertaLaboral.FechaCierre,
                        TipoContrato = p.OfertaLaboral.IdTipoContratoNavigation.NombreTipo,
                        Modalidad = p.OfertaLaboral.IdModalidadTrabajoNavigation.NombreModalidad,

                        EmpresaRuc = p.OfertaLaboral.IdEmpresaNavigation.Ruc,
                        EmpresaRazonSocial = p.OfertaLaboral.IdEmpresaNavigation.RazonSocial,
                        EmpresaTelefono = p.OfertaLaboral.IdEmpresaNavigation.Telefono,
                        EmpresaCorreo = p.OfertaLaboral.IdEmpresaNavigation.Correo
                    }
                })
                .OrderByDescending(x => x.FechaPostulacion) // Las más recientes primero
                .ToListAsync();

            return Ok(misPostulaciones);
        }

        // Postular
        [HttpPost("postular")]
        public async Task<ActionResult> Postular([FromBody] CrearPostulacionDto request)
        {
            var idEgresado = await _usuarioActualService.GetIdEgresadoActualAsync();
            if (idEgresado == null) return Unauthorized("Usuario no es egresado.");

            // 1. Validar Oferta
            var oferta = await _context.OfertasLaborales
                .FirstOrDefaultAsync(x => x.IdOferta == request.IdOferta && x.Estado == true);

            if (oferta == null) return BadRequest("La oferta no existe o no está activa.");
            if (oferta.FechaCierre < DateOnly.FromDateTime(DateTime.Now))
                return BadRequest("La oferta ha finalizado.");

            // 2. Validar postulación existente
            var postulacionExistente = await _context.Postulaciones
                .FirstOrDefaultAsync(p => p.IdOferta == request.IdOferta && p.IdEgresado == idEgresado);

            if (postulacionExistente != null)
            {
                if (postulacionExistente.Estado == "Cancelado")
                    return BadRequest("Cancelaste tu postulación anteriormente, no puedes volver a postular.");

                return BadRequest("Ya tienes una postulación activa para esta oferta.");
            }

            // 3. Crear Postulación
            var nuevaPostulacion = new Postulacion
            {
                IdOferta = request.IdOferta,
                IdEgresado = idEgresado.Value,
                FechaPostulacion = DateTime.Now,
                Estado = "Pendiente",
                CartaPresentacion = request.CartaPresentacion
            };

            _context.Postulaciones.Add(nuevaPostulacion);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Postulación enviada correctamente." });
        }

        // Cancelar Postulación
        [HttpDelete("cancelar/{idOferta:int}")]
        public async Task<ActionResult> CancelarPostulacion(int idOferta)
        {
            var idEgresado = await _usuarioActualService.GetIdEgresadoActualAsync();
            if (idEgresado == null) return Unauthorized("Usuario no es egresado.");

            var postulacion = await _context.Postulaciones
                .FirstOrDefaultAsync(p => p.IdOferta == idOferta && p.IdEgresado == idEgresado);

            if (postulacion == null) return NotFound("No has postulado a esta oferta.");
            if (postulacion.Estado == "Cancelado") return BadRequest("Ya estaba cancelada.");

            postulacion.Estado = "Cancelado";
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ==========================================
        // SECCIÓN REPRESENTANTE
        // ==========================================

        // Ver postulaciones de una oferta específica
        [HttpGet("por-oferta/{idOferta:int}")]
        public async Task<ActionResult<List<PostulanteDetalleDto>>> GetPostulacionesPorOferta(int idOferta)
        {
            var idRepresentante = await _usuarioActualService.GetIdRepresentanteActualAsync();
            if (idRepresentante == null) return Unauthorized("Usuario no es representante.");

            var representante = await _context.Representantes.FindAsync(idRepresentante);

            // Verificar que la oferta sea de la empresa del representante
            var oferta = await _context.OfertasLaborales.FindAsync(idOferta);
            if (oferta == null || oferta.IdEmpresa != representante.IdEmpresa)
                return Forbid("No tienes acceso a esta oferta.");

            var postulaciones = await _context.Postulaciones
                .Where(p => p.IdOferta == idOferta && p.Estado != "Cancelado")
                .Include(p => p.Egresado)
                    .ThenInclude(e => e.ExperienciaLaborals)
                .Include(p => p.Egresado)
                    .ThenInclude(e => e.Persona)
                .Include(e => e.Egresado)
                    .ThenInclude(e => e.FormacionComplementaria)
                .Select(p => new PostulanteDetalleDto
                {
                    IdPostulacion = p.IdPostulacion,
                    CartaPresentacion = p.CartaPresentacion,
                    FechaPostulacion = p.FechaPostulacion,
                    NombreCompleto = p.Egresado.Persona.Nombres + " " 
                        + p.Egresado.Persona.ApellidoPaterno + " " 
                        + p.Egresado.Persona.ApellidoMaterno,
                    Dni = p.Egresado.Persona.DocumentoIdentidad,
                    Correo = p.Egresado.Persona.CorreoPersonal,
                    Telefono = p.Egresado.Persona.Telefono,
                    ExperienciaLaboral = p.Egresado.ExperienciaLaborals.Select(exp => new ExperienciaLaboralDto
                    {
                        Empresa = exp.Empresa,
                        Cargo = exp.Cargo,
                        FechaInicio = exp.FechaInicio,
                        FechaFin = exp.FechaFin,
                        Descripcion = exp.Descripcion
                    })
                    //falta de formacion complementaria
                    .ToList(),
                })
                .ToListAsync();

            return Ok(postulaciones);
        }

        // Aceptar Postulación
        [HttpPost("aceptar")]
        public async Task<ActionResult> AceptarPostulacion([FromBody] EvaluarPostulacionDto evaluacion)
        {
            // TODO: Implementar lógica de aceptación según apuntes:
            // 1. Verificar usuario representante.
            // 2. Obtener postulación.
            // 3. Verificar estado != 'Cancelado'.
            // 4. Cambiar estado a 'Aceptado'.
            // 5. Guardar ID representante y comentarios.

            return StatusCode(501, "Método no implementado aún");
        }

        // Rechazar Postulación
        [HttpPost("rechazar")]
        public async Task<ActionResult> RechazarPostulacion([FromBody] EvaluarPostulacionDto evaluacion)
        {
            // TODO: Implementar lógica de rechazo según apuntes:
            // 1. Verificar usuario representante.
            // 2. Obtener postulación.
            // 3. Verificar estado != 'Cancelado'.
            // 4. Cambiar estado a 'Rechazado'.
            // 5. Guardar ID representante y comentarios.

            return StatusCode(501, "Método no implementado aún");
        }
    }
}
