using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Services;
using System.Data;

namespace SiseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly SiseDbContext _context;
        private readonly IUsuarioActualService _usuarioActualService;

        public UsuarioController(SiseDbContext context, IUsuarioActualService usuarioActualService)
        {
            _context = context;
            _usuarioActualService = usuarioActualService;
        }

        [HttpGet("UsernameEstaDisponible/{Username}")]
        public async Task<ActionResult<bool>> UsernameEstaDisponible(string Username)
        {
            var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            try
            {
                var paramSalida = new SqlParameter("@Disponible", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };

                await _context.Database.ExecuteSqlRawAsync(
                    @"EXEC sp_Username_Esta_Disponible @Username, @Disponible OUTPUT",
                    new SqlParameter("@Username", Username),
                    paramSalida);

                return Ok((bool)paramSalida.Value);
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

        [HttpPost("CambiarPassword")]
        public async Task<ActionResult> CambiarPassword([FromBody] string dto) //! falta implementar
        {
            return Ok();
        }
    }
}
