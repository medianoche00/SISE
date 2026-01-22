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
    public class EgresadoController : ControllerBase
    {
        private readonly SiseDbContext _context;
        private readonly IUsuarioActualService _usuarioActualService;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public EgresadoController(SiseDbContext context, IUsuarioActualService usuarioActualService, UserManager<IdentityUser<int>> userManager)
        {
            _context = context;
            _usuarioActualService = usuarioActualService;
            _userManager = userManager;
        }

        [HttpGet("{IdEgresado:int}")]
        public async Task<ActionResult<EgresadoDto>> GetEgresadosPorId(int IdEgresado)
        {
            var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            try
            {
                var resultado = await _context.Database.SqlQueryRaw<EgresadoDto>(
                    @"EXEC sp_Egresado_Por_Id @IdEgresado",
                    new SqlParameter("@IdEgresado", IdEgresado)
                ).ToListAsync();

                var egresado = resultado.FirstOrDefault();

                return Ok(egresado);
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
        public async Task<ActionResult> RegistrarEgresadoAsync([FromBody] EgresadoCrearDto dto)
        {

            if (string.IsNullOrWhiteSpace(dto.DocumentoRespaldo))
            {
                return BadRequest("Es obligatorio indicar el documento que respalda la operacion.");
            }

            var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            var idUsuario = _usuarioActualService.GetIdUsuarioLogueado();

            var user = new IdentityUser<int>
            {
                UserName = dto.Username,
                Email = dto.Email,
                EmailConfirmed = true
            };

            // Crear el usuario
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Error al crear usuario: {errors}");
            }

            try
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "Egresado");

                if (!roleResult.Succeeded)
                {
                    throw new Exception("Error al asignar el rol de Egresado.");
                }
                await _context.EjecutarSpConAuditoriaAsync(
                    dto.DocumentoRespaldo,
                    idUsuario,
                    @"EXEC sp_Egresado_Rol_Guardar
                    @IdPersona, @IdCarrera, @CodigoUniversitario,
                    @AnioEgreso, @IdUsuario",
                    new SqlParameter("@IdPersona", dto.IdPersona),
                    new SqlParameter("@IdCarrera", dto.IdCarrera),
                    new SqlParameter("@CodigoUniversitario", dto.CodigoUniversitario),
                    new SqlParameter("@AnioEgreso", dto.AnioEgreso),
                    new SqlParameter("@IdUsuario", user.Id)
                );

                return Ok();
            }
            catch (Exception ex)
            {
                // Si falló la asignación de rol o el SP de SQL, debemos BORRAR el usuario
                // para que no quede un usuario huerfano sin datos de egresado

                await _userManager.DeleteAsync(user);
                throw new Exception($"El registro falló y se revirtió el usuario. Detalle: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult> ActualizarEgresadoAsync([FromBody] EgresadoActualizarDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DocumentoRespaldo))
            {
                return BadRequest("Es obligatorio indicar el documento que respalda la operacion.");
            }

            var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            var idUsuario = _usuarioActualService.GetIdUsuarioLogueado();

            try
            {
                await _context.EjecutarSpConAuditoriaAsync(
                    dto.DocumentoRespaldo,
                    idUsuario,
                    @"EXEC sp_Egresado_Actualizar
                    @IdEgresado, @IdCarrera, @CodigoUniversitario, @AnioEgreso",
                    new SqlParameter("@IdEgresado", dto.IdEgresado),
                    new SqlParameter("@IdCarrera", dto.IdCarrera),
                    new SqlParameter("@CodigoUniversitario", dto.CodigoUniversitario),
                    new SqlParameter("@AnioEgreso", dto.AnioEgreso)
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

        [HttpDelete("{IdEgresado:int}")]
        public async Task<ActionResult> EliminarEgresadoAsync(int IdEgresado, [FromQuery] string DocumentoRespaldo)
        {
            if (string.IsNullOrWhiteSpace(DocumentoRespaldo))
            {
                return BadRequest("Es obligatorio indicar el documento que respalda la operacion.");
            }

            var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            var idUsuario = _usuarioActualService.GetIdUsuarioLogueado();

            try
            {
                await _context.EjecutarSpConAuditoriaAsync(
                    DocumentoRespaldo,
                    idUsuario,
                    @"EXEC sp_Egresado_Eliminar @IdEgresado",
                    new SqlParameter("@IdEgresado", IdEgresado)
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
