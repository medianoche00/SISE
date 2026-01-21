using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Models;
using SiseApi.Services;

namespace SiseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogosController : ControllerBase
    {
        private readonly SiseDbContext _context;
        private readonly IUsuarioActualService _usuarioActualService;

        public CatalogosController(SiseDbContext context, IUsuarioActualService usuarioActualService)
        {
            _context = context;
            _usuarioActualService = usuarioActualService;
        }

        // GET: api/Roles
        [HttpGet("Roles")]
        public async Task<ActionResult<List<RolDto>>> GetRoles()
        {
            //var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            //if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            try
            {
                var roles = await _context.Database.SqlQueryRaw<RolDto>(
                    "EXEC dbo.sp_Rol_Listar"
                ).ToListAsync();

                return Ok(roles);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error interno." + ex.Message);
            }
        }

        // GET: api/Carreras
        [HttpGet("Carreras")]
        public async Task<ActionResult<List<CarreraDto>>> GetCarreras()
        {
            //var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            //if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            try
            {
                var carreras = await _context.Database.SqlQueryRaw<CarreraDto>(
                    "EXEC dbo.sp_Carrera_Listar"
                ).ToListAsync();

                return Ok(carreras);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error interno." + ex.Message);
            }
        }

        // GET: api/Empresas
        [HttpGet("Empresas")]
        public async Task<ActionResult<List<EmpresaMinDto>>> GetEmpresas()
        {
            //var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            //if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            try
            {
                //usa EmpresaDimDto para listar solo los campos necesarios
                var empresas = await _context.Database.SqlQueryRaw<EmpresaMinDto>(
                    "EXEC dbo.sp_Empresa_Listar"
                ).ToListAsync();

                return Ok(empresas);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error interno." + ex.Message);
            }
        }

        // GET: api/CargosAdministrativos
        [HttpGet("CargosAdministrativos")]
        public async Task<ActionResult<List<CargoAdministrativoDto>>> GetCargosAdministrativos()
        {
            //var idAdministrador = await _usuarioActualService.GetIdAdministrativoActualAsync();
            //if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            try
            {
                //usa EmpresaDimDto para listar solo los campos necesarios
                var cargos = await _context.Database.SqlQueryRaw<CargoAdministrativoDto>(
                    "EXEC dbo.sp_CargoAdministrativo_Listar"
                ).ToListAsync();

                return Ok(cargos);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error interno." + ex.Message);
            }
        }
    }
}
