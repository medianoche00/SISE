using Microsoft.AspNetCore.Mvc;
using SiseApi.Services;

namespace SiseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly ReportesService _reportesService;

        public ReportesController(ReportesService reportesService)
        {
            _reportesService = reportesService;
        }

        [HttpGet("dashboard-completo")]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                var resultado = await _reportesService.ObtenerDashboardCompleto();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                // Esto nos ayudará si falla algún nombre de campo (como 'Estado' o 'NombreComercial')
                return StatusCode(500, "Error generando reporte: " + ex.Message);
            }
        }
    }
}   