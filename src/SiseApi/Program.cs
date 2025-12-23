using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Seed;
//using SiseApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Registrar DBContext con la cadena de conexión
builder.Services.AddDbContext<SiseDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar seeder
builder.Services.AddScoped<IDbSeeder, DbSeeder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Aplicar migraciones y ejecutar seed al iniciar en Desarrollo
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<SiseDbContext>();
        db.Database.Migrate(); // aplica migraciones pendientes

        // opcional: ejecutar un seeder
        var seeder = services.GetService<IDbSeeder>();
        seeder?.SeedAsync().GetAwaiter().GetResult();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error al aplicar migraciones/seed en Development");
        throw;
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
