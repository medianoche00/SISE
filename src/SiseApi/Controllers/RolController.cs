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
    public class RolController : ControllerBase
    {
        private readonly SiseDbContext _context;
        private readonly IUsuarioActualService _usuarioActualService;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public RolController(SiseDbContext context, IUsuarioActualService usuarioActualService, UserManager<IdentityUser<int>> userManager)
        {
            _context = context;
            _usuarioActualService = usuarioActualService;
            _userManager = userManager;
        }

        [HttpPost("RegistrarEgresado")]
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

        [HttpPost("RegistrarAdministrativo")]
        public async Task<ActionResult> RegistrarAdministrativoAsync([FromBody] AdministrativoCrearDto dto)
        {
            // Validaciones iniciales
            if (string.IsNullOrWhiteSpace(dto.DocumentoRespaldo)) return BadRequest("Documento de respaldo obligatorio.");

            var idAdminEjecutor = await _usuarioActualService.GetIdAdministrativoActualAsync();
            if (idAdminEjecutor == null) return Unauthorized("No autorizado.");

            var user = new IdentityUser<int> { UserName = dto.Username, Email = dto.Email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded) throw new Exception($"Error: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            try
            {
                await _userManager.AddToRoleAsync(user, "Administrativo");

                await _context.EjecutarSpConAuditoriaAsync(
                    dto.DocumentoRespaldo,
                    _usuarioActualService.GetIdUsuarioLogueado(),
                    @"EXEC sp_Administrativo_Rol_Guardar @IdPersona, @IdCargoAdministrativo, @IdUsuario",
                    new SqlParameter("@IdPersona", dto.IdPersona),
                    new SqlParameter("@IdCargoAdministrativo", dto.IdCargoAdministrativo),
                    new SqlParameter("@IdUsuario", user.Id)
                );
                return Ok();
            }
            catch (Exception ex)
            {
                await _userManager.DeleteAsync(user);
                throw new Exception($"Error: {ex.Message}");
            }
        }

        [HttpPost("RegistrarAdministrador")]
        public async Task<ActionResult> RegistrarAdministradorAsync([FromBody] AdministrativoCrearDto dto)
        {
            // Validaciones iniciales
            if (string.IsNullOrWhiteSpace(dto.DocumentoRespaldo)) return BadRequest("Documento de respaldo obligatorio.");

            var idAdminEjecutor = await _usuarioActualService.GetIdAdministrativoActualAsync();
            if (idAdminEjecutor == null) return Unauthorized("No autorizado.");

            var user = new IdentityUser<int> { UserName = dto.Username, Email = dto.Email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded) throw new Exception($"Error: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            try
            {
                await _userManager.AddToRoleAsync(user, "Administrador");

                await _context.EjecutarSpConAuditoriaAsync(
                    dto.DocumentoRespaldo,
                    _usuarioActualService.GetIdUsuarioLogueado(),
                    @"EXEC sp_Administrativo_Rol_Guardar @IdPersona, @IdCargoAdministrativo, @IdUsuario",
                    new SqlParameter("@IdPersona", dto.IdPersona),
                    new SqlParameter("@IdCargoAdministrativo", dto.IdCargoAdministrativo),
                    new SqlParameter("@IdUsuario", user.Id)
                );
                return Ok();
            }
            catch (Exception ex)
            {
                await _userManager.DeleteAsync(user);
                throw new Exception($"Error: {ex.Message}");
            }
        }

        [HttpPost("RegistrarRepresentante")]
        public async Task<ActionResult> RegistrarRepresentanteAsync([FromBody] RepresentanteCrearDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DocumentoRespaldo)) return BadRequest("Documento de respaldo obligatorio.");

            var user = new IdentityUser<int> { UserName = dto.Username, Email = dto.Email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded) throw new Exception("Error al crear usuario Identity.");

            try
            {
                await _userManager.AddToRoleAsync(user, "Representante");

                await _context.EjecutarSpConAuditoriaAsync(
                    dto.DocumentoRespaldo,
                    _usuarioActualService.GetIdUsuarioLogueado(),
                    @"EXEC sp_Representante_Rol_Guardar @IdPersona, @IdEmpresa, @IdUsuario, @Cargo",
                    new SqlParameter("@IdPersona", dto.IdPersona),
                    new SqlParameter("@IdEmpresa", dto.IdEmpresa),
                    new SqlParameter("@IdUsuario", user.Id),
                    new SqlParameter("@Cargo", (object)dto.Cargo ?? DBNull.Value)
                );
                return Ok();
            }
            catch (Exception ex)
            {
                await _userManager.DeleteAsync(user);
                throw new Exception($"Error: {ex.Message}");
            }
        }
    }
}
