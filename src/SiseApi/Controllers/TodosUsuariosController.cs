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
            var usuarios = _db.Users
                .Select(u => new { u.Id, u.UserName })
                .ToList();

            return Ok(usuarios);
        }
    }
}
