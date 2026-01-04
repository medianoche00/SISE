using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Data.Models;

namespace SiseApi.Services
{
    public class ReportesService
    {
        private readonly SiseDbContext _context;

        public ReportesService(SiseDbContext context)
        {
            _context = context;
        }

        public async Task<object> ObtenerDashboardCompleto()
        {
            // --- PARTE 1: Egresados Trabajando Actualmente ---
            // Nota: Asumo que la tabla se llama ExperienciaLaborals o ExperienciasLaborales en tu DbContext.
            // Si marca error aquí, verifica en SiseDbContext.cs cómo se llama el DbSet.
            var totalEgresados = await _context.Egresados.CountAsync();

            var egresadosTrabajando = await _context.ExperienciasLaborales // Ojo: Verifica si es con 's' o 'es'
                .Where(e => e.FechaInicio != null && e.FechaFin == null)
                .Select(e => e.IdEgresado)
                .Distinct()
                .CountAsync();

            double porcentaje = totalEgresados > 0
                ? (double)egresadosTrabajando / totalEgresados * 100
                : 0;

            // --- PARTE 2: Estadística por Empresa ---
            var reporteEmpresas = await _context.Empresas
                .Select(emp => new
                {
                    Empresa = emp.RazonSocial, // CORREGIDO: Usamos RazonSocial
                    OfertasPublicadas = emp.OfertaLaborals.Count(), // CORREGIDO: Usamos OfertaLaborals

                    // CORREGIDO: Asumimos que la lista se llama "Postulacions" (patrón inglés)
                    // Si te marca error en 'Postulacions', cámbialo a 'Postulaciones'.
                    PuestosCubiertos = emp.OfertaLaborals
                        .SelectMany(o => o.Postulaciones)
                        .Count(p => p.Estado == "Contratado" || p.Estado == "Seleccionado")
                })
                .OrderByDescending(x => x.OfertasPublicadas)
                .ToListAsync();

            return new
            {
                ResumenGeneral = new
                {
                    TotalEgresados = totalEgresados,
                    ActualmenteTrabajando = egresadosTrabajando,
                    PorcentajeEmpleabilidad = Math.Round(porcentaje, 2) + "%"
                },
                DesempenoEmpresas = reporteEmpresas
            };
        }
    }
}