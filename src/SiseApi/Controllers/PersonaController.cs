using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Models;
using SiseApi.Services;
using System.Data;

namespace SiseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private readonly SiseDbContext _context;
        private readonly IUsuarioActualService _usuarioActualService;

        public PersonaController(SiseDbContext context, IUsuarioActualService usuarioActualService)
        {
            _context = context;
            _usuarioActualService = usuarioActualService;
        }

        // GET: api/Persona
        [HttpGet]
        public async Task<ActionResult<List<PersonaDto>>> GetAll()
        {
            var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            try
            {
                // Usamos PersonaDto para la salida
                var personas = await _context.Database.SqlQueryRaw<PersonaDto>(
                    "EXEC dbo.sp_Persona_Listar"
                ).ToListAsync();

                return Ok(personas);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        // GET: api/Persona/5
        [HttpGet("{idPersona:int}")]
        public async Task<ActionResult<PersonaDto>> GetById(int idPersona)
        {
            var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            try
            {
                // Lo ideal sería: "EXEC sp_Persona_ObtenerPorId @IdPersona"
                var resultado = await _context.Database.SqlQueryRaw<PersonaDto>(
                    "EXEC dbo.sp_Persona_Listar" // Si este lista todo, filtramos en memoria (no ideal para mucha data) o crea un SP especifico
                ).ToListAsync();

                var persona = resultado.FirstOrDefault(p => p.IdPersona == idPersona);

                if (persona == null) return NotFound("Persona no encontrada.");

                return Ok(persona);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Persona
        [HttpPost]
        public async Task<ActionResult> CrearPersona([FromBody] PersonaCrearDto dto)
        {

            var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            var idUsuario = _usuarioActualService.GetIdUsuarioLogueado();

            try
            {
                var paramIdSalida = new SqlParameter("@IdPersonaGenerado", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                await _context.EjecutarSpConAuditoriaAsync(
                    dto.DocumentoRespaldo,
                    idUsuario,
                    @"EXEC sp_Persona_Insertar 
                    @Nombres, @ApellidoPaterno, @ApellidoMaterno, 
                    @IdTipoDocumento, @NumeroDocumento, 
                    @Telefono, @CorreoPersonal,
                    @IdDistrito, @Calle, @Numero, @PisoDepartamento, @Referencia,
                    @IdPersonaGenerado OUTPUT",

                    new SqlParameter("@Nombres", dto.Nombres),
                    new SqlParameter("@ApellidoPaterno", dto.ApellidoPaterno),
                    new SqlParameter("@ApellidoMaterno", dto.ApellidoMaterno),
                    new SqlParameter("@IdTipoDocumento", dto.IdTipoDocumento),
                    new SqlParameter("@NumeroDocumento", dto.NumeroDocumento),
                    new SqlParameter("@Telefono", (object?)dto.Telefono ?? DBNull.Value),
                    new SqlParameter("@CorreoPersonal", (object?)dto.CorreoPersonal ?? DBNull.Value),

                    new SqlParameter("@IdDistrito", dto.IdDistrito),
                    new SqlParameter("@Calle", dto.Calle),
                    new SqlParameter("@Numero", (object?)dto.Numero ?? DBNull.Value),
                    new SqlParameter("@PisoDepartamento", (object?)dto.PisoDepartamento ?? DBNull.Value),
                    new SqlParameter("@Referencia", (object?)dto.Referencia ?? DBNull.Value),

                    paramIdSalida
                );

                int idGenerado = (int)paramIdSalida.Value;

                return CreatedAtAction(nameof(GetById), new { idPersona = idGenerado }, new { id = idGenerado });
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear persona: {ex.Message}");
            }
        }

        // PUT: api/Persona
        [HttpPut]
        public async Task<ActionResult> ActualizarPersona([FromBody] PersonaActualizarDto dto)
        {
            var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            var idUsuario = _usuarioActualService.GetIdUsuarioLogueado();

            try
            {
                await _context.EjecutarSpConAuditoriaAsync(
                    dto.DocumentoRespaldo, // Viene dentro del objeto JSON
                    idUsuario,
                    @"EXEC sp_Persona_Actualizar 
                    @IdPersona, 
                    @Nombres, @ApellidoPaterno, @ApellidoMaterno, 
                    @IdTipoDocumento, @NumeroDocumento, 
                    @Telefono, @CorreoPersonal,
                    @IdDistrito, @Calle, @Numero, @PisoDepartamento, @Referencia, @Estado",

                    new SqlParameter("@IdPersona", dto.IdPersona),
                    new SqlParameter("@Nombres", dto.Nombres),
                    new SqlParameter("@ApellidoPaterno", dto.ApellidoPaterno),
                    new SqlParameter("@ApellidoMaterno", dto.ApellidoMaterno),
                    new SqlParameter("@IdTipoDocumento", dto.IdTipoDocumento),
                    new SqlParameter("@NumeroDocumento", dto.NumeroDocumento),
                    new SqlParameter("@Telefono", (object?)dto.Telefono ?? DBNull.Value),
                    new SqlParameter("@CorreoPersonal", (object?)dto.CorreoPersonal ?? DBNull.Value),

                    new SqlParameter("@IdDistrito", dto.IdDistrito),
                    new SqlParameter("@Calle", dto.Calle),
                    new SqlParameter("@Numero", (object?)dto.Numero ?? DBNull.Value),
                    new SqlParameter("@PisoDepartamento", (object?)dto.PisoDepartamento ?? DBNull.Value),
                    new SqlParameter("@Referencia", (object?)dto.Referencia ?? DBNull.Value),
                    new SqlParameter("@Estado", dto.Estado)
                );

                return Ok(new { message = "Persona actualizada correctamente" });
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar persona: {ex.Message}");
            }
        }

        // DELETE: api/Persona/{id}
        // Nota: Para eliminar mantenemos el DocumentoRespaldo por QueryString ya que DELETE usualmente no lleva Body
        [HttpDelete("{idPersona:int}")]
        public async Task<ActionResult> EliminarPersona(int idPersona, [FromQuery] string documentoRespaldo)
        {
            if (string.IsNullOrWhiteSpace(documentoRespaldo))
            {
                return BadRequest("Es obligatorio indicar el documento que respalda la eliminación.");
            }

            var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            var idUsuario = _usuarioActualService.GetIdUsuarioLogueado();

            try
            {
                await _context.EjecutarSpConAuditoriaAsync(
                    documentoRespaldo,
                    idUsuario,
                    "EXEC sp_Persona_Eliminar @IdPersona",
                    new SqlParameter("@IdPersona", idPersona)
                );
                return Ok(new { message = "Persona eliminada correctamente" });
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar persona: {ex.Message}");
            }
        }
    }
}