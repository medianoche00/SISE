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
    public class ExpedienteController : ControllerBase
    {
        private readonly SiseDbContext _context;
        private readonly IUsuarioActualService _usuarioActualService;

        public ExpedienteController(SiseDbContext context, IUsuarioActualService usuarioActualService)
        {
            _context = context;
            _usuarioActualService = usuarioActualService;
        }

        [HttpGet("{idPersona:int}")]
        public async Task<ActionResult<ExpedienteDto>> GetExpediente(int idPersona)
        {
            var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            try
            {
                var expedientes = await _context.Database.SqlQueryRaw<ExpedienteDto>(
                    "EXEC dbo.sp_Usuarios_De_Persona_Listar @idPersona"
                    , new SqlParameter("@idPersona", idPersona)
                ).ToListAsync();

                return Ok(expedientes);
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
    }
}
