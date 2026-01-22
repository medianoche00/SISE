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
    public class AdministrativoController : ControllerBase
    {
        private readonly SiseDbContext _context;
        private readonly IUsuarioActualService _usuarioActualService;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public AdministrativoController(SiseDbContext context, IUsuarioActualService usuarioActualService, UserManager<IdentityUser<int>> userManager)
        {
            _context = context;
            _usuarioActualService = usuarioActualService;
            _userManager = userManager;
        }

        [HttpGet("{IdAdministrativo:int}")]
        public async Task<ActionResult<AdministrativoDto>> GetAdministrativoPorId(int IdAdministrativo)
        {
            // Solo un administrador puede ver detalles de otros administrativos
            var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            if (idAdministrador == null) return Unauthorized("Usuario no autorizado.");

            try
            {
                var resultado = await _context.Database.SqlQueryRaw<AdministrativoDto>(
                    @"EXEC sp_Administrativo_Por_Id @IdAdministrativo",
                    new SqlParameter("@IdAdministrativo", IdAdministrativo)
                ).ToListAsync();

                var administrativo = resultado.FirstOrDefault();

                if (administrativo == null) return NotFound("Administrativo no encontrado.");

                return Ok(administrativo);
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
        public async Task<ActionResult> RegistrarAdministrativoAsync([FromBody] AdministrativoCrearDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DocumentoRespaldo))
            {
                return BadRequest("Es obligatorio indicar el documento que respalda la operación.");
            }

            // Validación de seguridad: Solo un administrativo actual puede crear otro
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
                // 2. Asignar el rol de Administrativo
                // Nota: Asegúrate de que el rol "Administrativo" exista en tu tabla AspNetRoles
                var roleResult = await _userManager.AddToRoleAsync(user, "Administrativo");

                if (!roleResult.Succeeded)
                {
                    throw new Exception("Error al asignar el rol de Administrativo.");
                }

                // 3. Ejecutar SP para guardar datos de negocio con auditoría
                await _context.EjecutarSpConAuditoriaAsync(
                    dto.DocumentoRespaldo,
                    idUsuarioLogueado,
                    @"EXEC sp_Administrativo_Crear
                @IdCargoAdministrativo, @IdPersona, @IdUsuario",
                    new SqlParameter("@IdCargoAdministrativo", dto.IdCargoAdministrativo),
                    new SqlParameter("@IdPersona", dto.IdPersona),
                    new SqlParameter("@IdUsuario", user.Id)
                );

                return Ok();
            }
            catch (Exception ex)
            {
                // Rollback manual: Si falla el SP o la asignación de rol, borramos el usuario creado
                await _userManager.DeleteAsync(user);

                if (ex.InnerException is SqlException sqlEx)
                {
                    return BadRequest(sqlEx.Message);
                }
                return StatusCode(500, $"El registro falló y se revirtió el usuario. Detalle: {ex.Message}");
            }
        }

        [HttpPost("Administrador")]
        public async Task<ActionResult> RegistrarAdministradorAsync([FromBody] AdministrativoCrearDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DocumentoRespaldo))
            {
                return BadRequest("Es obligatorio indicar el documento que respalda la operación.");
            }

            // Validación de seguridad: Solo un administrativo actual puede crear otro
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
                // 2. Asignar el rol de Administrativo
                // Nota: Asegúrate de que el rol "Administrativo" exista en tu tabla AspNetRoles
                var roleResult = await _userManager.AddToRoleAsync(user, "Administrador");

                if (!roleResult.Succeeded)
                {
                    throw new Exception("Error al asignar el rol de Administrativo.");
                }

                // 3. Ejecutar SP para guardar datos de negocio con auditoría
                await _context.EjecutarSpConAuditoriaAsync(
                    dto.DocumentoRespaldo,
                    idUsuarioLogueado,
                    @"EXEC sp_Administrativo_Crear
                @IdCargoAdministrativo, @IdPersona, @IdUsuario",
                    new SqlParameter("@IdCargoAdministrativo", dto.IdCargoAdministrativo),
                    new SqlParameter("@IdPersona", dto.IdPersona),
                    new SqlParameter("@IdUsuario", user.Id)
                );

                return Ok();
            }
            catch (Exception ex)
            {
                // Rollback manual: Si falla el SP o la asignación de rol, borramos el usuario creado
                await _userManager.DeleteAsync(user);

                if (ex.InnerException is SqlException sqlEx)
                {
                    return BadRequest(sqlEx.Message);
                }
                return StatusCode(500, $"El registro falló y se revirtió el usuario. Detalle: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult> ActualizarAdministrativoAsync([FromBody] AdministrativoActualizarDto dto)
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
                    @"EXEC sp_Administrativo_Actualizar
                @IdAdministrativo, @IdCargoAdministrativo",
                    new SqlParameter("@IdAdministrativo", dto.IdAdministrativo),
                    new SqlParameter("@IdCargoAdministrativo", dto.IdCargoAdministrativo)
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

        [HttpDelete("{IdAdministrativo:int}")]
        public async Task<ActionResult> EliminarAdministrativoAsync(int IdAdministrativo, [FromQuery] string DocumentoRespaldo)
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
                    @"EXEC sp_Administrativo_Eliminar @IdAdministrativo",
                    new SqlParameter("@IdAdministrativo", IdAdministrativo)
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