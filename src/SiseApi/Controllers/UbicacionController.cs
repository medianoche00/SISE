using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Models;
using SiseApi.Services;

namespace SiseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UbicacionController : ControllerBase
    {
        private readonly SiseDbContext _context;
        private readonly IUsuarioActualService _usuarioActualService;

        public UbicacionController(SiseDbContext context, IUsuarioActualService usuarioActualService)
        {
            _context = context;
            _usuarioActualService = usuarioActualService;
        }

        // GET: api/Ubicacion
        [HttpGet]
        public async Task<ActionResult> GetAll() {
            // obtener una lista plana con joins de las 3 tablas Departamento, Provincia, Distrito
            var dataPlana = await _context.Database
                .SqlQueryRaw<UbicacionFlatDto>("EXEC dbo.sp_Ubicacion_ListarCompleto")
                .ToListAsync();

            //transformacion de la lista a un formato jerarquico
            var arbolUbicaciones = dataPlana
                .GroupBy(d => new { d.IdDepartamento, d.NombreDepartamento })
                .Select(gDept => new DepartamentoDto
                {
                    Id = gDept.Key.IdDepartamento,
                    Nombre = gDept.Key.NombreDepartamento,
                    Provincias = gDept
                        .GroupBy(p => new { p.IdProvincia, p.NombreProvincia })
                        .Select(gProv => new ProvinciaDto
                        {
                            Id = gProv.Key.IdProvincia,
                            Nombre = gProv.Key.NombreProvincia,
                            Distritos = gProv
                                .Select(dis => new DistritoDto
                                {
                                    Id = dis.IdDistrito,
                                    Nombre = dis.NombreDistrito
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();

            return Ok(arbolUbicaciones);
        }
    }
}
