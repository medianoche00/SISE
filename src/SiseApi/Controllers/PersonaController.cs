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
                var personas = await _context.Database.SqlQueryRaw<PersonaListDto>(
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
                return StatusCode(500, "Ocurrió un error interno al obtener las personas.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearPersona([FromBody] PersonaDto personaDto)
        {
            if (string.IsNullOrWhiteSpace(personaDto.DocumentoRespaldo))
            {
                return BadRequest("Es obligatorio indicar el documento que respalda la eliminación.");
            }

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
                    personaDto.DocumentoRespaldo,
                    idUsuario,
                    @"EXEC sp_Persona_Insertar 
                    @Nombres, @ApellidoPaterno, @ApellidoMaterno, 
                    @IdTipoDocumento, @NumeroDocumento, 
                    @Telefono, @CorreoPersonal,
                    @IdDistrito, @Calle, @Numero, @PisoDepartamento, @Referencia,
                    @IdPersonaGenerado OUTPUT",

                    new SqlParameter("@Nombres", personaDto.Nombres),
                    new SqlParameter("@ApellidoPaterno", personaDto.ApellidoPaterno),
                    new SqlParameter("@ApellidoMaterno", personaDto.ApellidoMaterno),
                    new SqlParameter("@IdTipoDocumento", personaDto.IdTipoDocumento),
                    new SqlParameter("@NumeroDocumento", personaDto.NumeroDocumento),
                    new SqlParameter("@Telefono", (object?)personaDto.Telefono ?? DBNull.Value),
                    new SqlParameter("@CorreoPersonal", (object?)personaDto.CorreoPersonal ?? DBNull.Value),

                    new SqlParameter("@IdDistrito", personaDto.IdDistrito),
                    new SqlParameter("@Calle", personaDto.Calle),
                    new SqlParameter("@Numero", (object?)personaDto.Numero ?? DBNull.Value),
                    new SqlParameter("@PisoDepartamento", (object?)personaDto.PisoDepartamento ?? DBNull.Value),
                    new SqlParameter("@Referencia", (object?)personaDto.Referencia ?? DBNull.Value),

                    paramIdSalida // parámetro de salida
                );

                int idGenerado = (int)paramIdSalida.Value;

                return CreatedAtAction(nameof(GetAll), new { id = idGenerado }, idGenerado);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error interno.");
            }
        }

        // PUT: api/Persona/{idPersona}
        [HttpPut("{idPersona:int}")]
        public async Task<ActionResult> ActualizarPersona(int idPersona, [FromBody] PersonaDto personaDto)
        {
            if (idPersona != personaDto.IdPersona) return BadRequest("El ID de la URL no coincide con el cuerpo.");

            if (string.IsNullOrWhiteSpace(personaDto.DocumentoRespaldo))
            {
                return BadRequest("Es obligatorio indicar el documento que respalda la eliminación.");
            }

            var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            var idUsuario = _usuarioActualService.GetIdUsuarioLogueado();

            try
            {
                // Nota: Se ha actualizado la cadena EXEC y los parámetros para incluir la dirección plana
                await _context.EjecutarSpConAuditoriaAsync(
                    personaDto.DocumentoRespaldo,
                    idUsuario,
                    @"EXEC sp_Persona_Actualizar 
                    @IdPersona, 
                    @Nombres, @ApellidoPaterno, @ApellidoMaterno, 
                    @IdTipoDocumento, @NumeroDocumento, 
                    @Telefono, @CorreoPersonal,
                    @IdDistrito, @Calle, @Numero, @PisoDepartamento, @Referencia",

                    new SqlParameter("@IdPersona", personaDto.IdPersona),
                    new SqlParameter("@Nombres", personaDto.Nombres),
                    new SqlParameter("@ApellidoPaterno", personaDto.ApellidoPaterno),
                    new SqlParameter("@ApellidoMaterno", personaDto.ApellidoMaterno),
                    new SqlParameter("@IdTipoDocumento", personaDto.IdTipoDocumento),
                    new SqlParameter("@NumeroDocumento", personaDto.NumeroDocumento),
                    new SqlParameter("@Telefono", (object?)personaDto.Telefono ?? DBNull.Value),
                    new SqlParameter("@CorreoPersonal", (object?)personaDto.CorreoPersonal ?? DBNull.Value),

                    new SqlParameter("@IdDistrito", personaDto.IdDistrito),
                    new SqlParameter("@Calle", personaDto.Calle),
                    new SqlParameter("@Numero", (object?)personaDto.Numero ?? DBNull.Value),
                    new SqlParameter("@PisoDepartamento", (object?)personaDto.PisoDepartamento ?? DBNull.Value),
                    new SqlParameter("@Referencia", (object?)personaDto.Referencia ?? DBNull.Value)
                );

                return NoContent();
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error interno.");
            }
        }

        // DELETE: api/Persona/{idPersona}
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
                return NoContent();
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error interno.");
            }
        }
    }
}
