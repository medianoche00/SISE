using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiseApi.Data;
using SiseApi.Models;

namespace SiseApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodosUsuariosController : Controller
    {
        private SiseDbContext _db;
        public TodosUsuariosController(SiseDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var usuarios = _db.Usuarios
                .Select(u => new { u.IdUsuario, u.Nomusuario, u.IdRol, u.Estado })
                .ToList();

            return Ok(usuarios);
        }
    }
}
