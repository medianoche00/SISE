using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SiseApi.Data;
using SiseApi.Seed;
using System;
using System.Threading.Tasks;

namespace SiseApi.Extensions
{
    public static class HostExtensions
    {
        public static async Task<IHost> MigrateAndSeedAsync(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                var context = services.GetRequiredService<SiseDbContext>();
                var seeder = services.GetRequiredService<IDbSeeder>();

                try
                {
                    logger.LogInformation("Iniciando migración de base de datos...");
                    await context.Database.MigrateAsync();

                    logger.LogInformation("Iniciando Seeding de datos...");
                    await seeder.SeedAsync();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Ocurrió un error durante la migración o el seeding de datos.");
                    // En entornos productivos, podrías querer detener la app aquí o manejarlo diferente
                }
            }
            return host;
        }
    }
}