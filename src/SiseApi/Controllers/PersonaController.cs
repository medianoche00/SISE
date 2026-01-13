using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Data.Models;
using SiseApi.Models;
using SiseApi.Services;

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

        public async Task<ActionResult<List<PersonaDto>>> Get()
        {
            //var idAdministrador = _usuarioActualService.GetIdAdministrativoActualAsync();
            //if (idAdministrador == null) return Unauthorized("Usuario no es administrador.");

            var personas = await _context.Database.SqlQueryRaw<PersonaDto>(
                "EXEC dbo.sp_Persona_Listar"
            ).ToListAsync();

            return Ok(personas);
        }
    }
}
