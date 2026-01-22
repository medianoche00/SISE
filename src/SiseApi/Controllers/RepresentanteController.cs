using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Models;
using SiseApi.Services;

namespace SiseApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class RepresentanteController : ControllerBase
    {
        private readonly SiseDbContext _context;
        private readonly IUsuarioActualService _usuarioActualService;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public RepresentanteController(SiseDbContext context, IUsuarioActualService usuarioActualService, UserManager<IdentityUser<int>> userManager)
        {
            _context = context;
            _usuarioActualService = usuarioActualService;
            _userManager = userManager;
        }

        [HttpGet("{IdRepresentante:int}")]
        public async Task<ActionResult<RepresentanteDto>> GetRepresentantePorId(int IdRepresentante)
        {
            var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            try
            {
                var representante = await _context.Database.SqlQueryRaw<RepresentanteDto>(
                    @"EXEC sp_Representante_Por_Id @IdRepresentante",
                    new SqlParameter("@IdRepresentante", IdRepresentante)
                ).FirstOrDefaultAsync();

                if (representante == null) return NotFound("Representante no encontrado.");

                return Ok(representante);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> RegistrarRepresentanteAsync([FromBody] RepresentanteCrearDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DocumentoRespaldo))
            {
                return BadRequest("Es obligatorio indicar el documento que respalda la operación.");
            }

            var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            var idUsuarioLogueado = _usuarioActualService.GetIdUsuarioLogueado();

            // 1. Crear el usuario en Identity
            var user = new IdentityUser<int>
            {
                UserName = dto.Username,
                Email = dto.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Error al crear usuario: {errors}");
            }

            try
            {
                // 2. Asignar el rol de Representante
                var roleResult = await _userManager.AddToRoleAsync(user, "Representante");

                if (!roleResult.Succeeded)
                {
                    throw new Exception("Error al asignar el rol de Representante.");
                }

                // 3. Ejecutar SP para guardar datos de negocio con auditoría
                await _context.EjecutarSpConAuditoriaAsync(
                    dto.DocumentoRespaldo,
                    idUsuarioLogueado,
                    @"EXEC sp_Representante_Crear
                @IdEmpresa, @IdPersona, @IdUsuario, @Cargo",
                    new SqlParameter("@IdEmpresa", dto.IdEmpresa),
                    new SqlParameter("@IdPersona", dto.IdPersona),
                    new SqlParameter("@IdUsuario", user.Id), // ID generado por Identity
                    new SqlParameter("@Cargo", dto.Cargo ?? (object)DBNull.Value)
                );

                return Ok();
            }
            catch (Exception ex)
            {
                // Si falla el SP o la asignación de rol, revertimos la creación del usuario
                // para mantener la integridad de los datos.
                await _userManager.DeleteAsync(user);

                // Retornamos el error original (ya sea SQL o de lógica)
                // Si es SqlException lo capturará el middleware global o se puede manejar aquí si prefieres
                if (ex.InnerException is SqlException sqlEx)
                {
                    return BadRequest(sqlEx.Message);
                }
                return StatusCode(500, $"El registro falló y se revirtió el usuario. Detalle: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult> ActualizarRepresentanteAsync([FromBody] RepresentanteActualizarDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DocumentoRespaldo))
            {
                return BadRequest("Es obligatorio indicar el documento que respalda la operación.");
            }

            var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            var idUsuarioLogueado = _usuarioActualService.GetIdUsuarioLogueado();

            try
            {
                await _context.EjecutarSpConAuditoriaAsync(
                    dto.DocumentoRespaldo,
                    idUsuarioLogueado,
                    @"EXEC sp_Representante_Actualizar
                    @IdRepresentante, @IdEmpresa, @Cargo",
                    new SqlParameter("@IdRepresentante", dto.IdRepresentante),
                    new SqlParameter("@IdEmpresa", dto.IdEmpresa),
                    new SqlParameter("@Cargo", dto.Cargo ?? (object)DBNull.Value)
                );

                return Ok();
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("{IdRepresentante:int}")]
        public async Task<ActionResult> EliminarRepresentanteAsync(int IdRepresentante, [FromQuery] string DocumentoRespaldo)
        {
            if (string.IsNullOrWhiteSpace(DocumentoRespaldo))
            {
                return BadRequest("Es obligatorio indicar el documento que respalda la operación.");
            }

            var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            var idUsuarioLogueado = _usuarioActualService.GetIdUsuarioLogueado();

            try
            {
                await _context.EjecutarSpConAuditoriaAsync(
                    DocumentoRespaldo,
                    idUsuarioLogueado,
                    @"EXEC sp_Representante_Eliminar @IdRepresentante",
                    new SqlParameter("@IdRepresentante", IdRepresentante)
                );
                return Ok();
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}