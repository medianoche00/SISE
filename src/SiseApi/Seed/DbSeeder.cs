using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Data.Models;
using SiseApi.Seed;
using System.Text.Json;
using System.Text.Json.Serialization;

// Asegúrate de importar los namespaces de tus Modelos y tu Contexto
// using TuProyecto.Models;
// using TuProyecto.Data;

public class DbSeeder : IDbSeeder
{
    private readonly SiseDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<DbSeeder> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public DbSeeder(
        SiseDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IWebHostEnvironment env,
        ILogger<DbSeeder> logger)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _env = env;
        _logger = logger;

        // Configuración para leer DateOnly y evitar errores de mayúsculas/minúsculas
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // 1. Tablas Maestras / Catálogos (Sin dependencias)
            await SeedRolesAsync();
            await SeedFacultadesAsync();
            await SeedModalidadTrabajoAsync();
            await SeedTipoContratoAsync();
            await SeedTipoFormacionAsync();

            // 2. Tablas con Dependencias de Nivel 1
            await SeedEscuelasAsync(); // Depende de Facultad
            await SeedEmpresasAsync();
            await SeedPersonasAsync();

            // 3. Tablas con Dependencias de Nivel 2
            await SeedCarrerasAsync(); // Depende de Escuela
            await SeedUsersAsync();    // Depende de Roles (y opcionalmente vincula personas)

            // Guardar cualquier cambio pendiente que no se haya guardado en los métodos individuales
            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync(cancellationToken);
            }

            _logger.LogInformation("Proceso de Seeding completado exitosamente.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió un error crítico durante el proceso de Seeding.");
            throw; // Re-lanzar para detener el arranque si es necesario
        }
    }

    // --- MÉTODOS DE AYUDA ---

    private async Task<List<T>?> LoadJsonData<T>(string fileName)
    {
        var path = Path.Combine(_env.ContentRootPath, "SeedData", fileName);
        if (!File.Exists(path))
        {
            _logger.LogWarning($"El archivo de seed '{fileName}' no fue encontrado en {path}. Se omitirá.");
            return null;
        }

        var jsonString = await File.ReadAllTextAsync(path);
        return JsonSerializer.Deserialize<List<T>>(jsonString, _jsonOptions);
    }

    // --- MÉTODOS ESPECÍFICOS POR ENTIDAD ---

    private async Task SeedRolesAsync()
    {
        // Archivo esperado: seed-roles.json
        // Estructura JSON sugerida: [{"Name": "Admin"}, {"Name": "Egresado"}, {"Name": "Empresa"}]
        var roles = await LoadJsonData<ApplicationRole>("seed-roles.json");
        if (roles == null) return;

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role.Name!))
            {
                await _roleManager.CreateAsync(new ApplicationRole(role.Name!));
                _logger.LogInformation($"Rol creado: {role.Name}");
            }
        }
    }

    private async Task SeedFacultadesAsync()
    {
        if (await _context.Facultades.AnyAsync()) return; // Evitar duplicados masivos

        var data = await LoadJsonData<Facultad>("seed-facultad.json");
        if (data != null)
        {
            await _context.Facultades.AddRangeAsync(data);
            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedEscuelasAsync()
    {
        if (await _context.Escuelas.AnyAsync()) return;

        var data = await LoadJsonData<Escuela>("seed-escuela.json");
        // Asegurarse de que las Facultades existan por ID o hacer un lookup aquí si el JSON no tiene IDs correctos
        if (data != null)
        {
            await _context.Escuelas.AddRangeAsync(data);
            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedCarrerasAsync()
    {
        if (await _context.Carreras.AnyAsync()) return;

        var data = await LoadJsonData<Carrera>("seed-carrera.json");
        if (data != null)
        {
            await _context.Carreras.AddRangeAsync(data);
            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedModalidadTrabajoAsync()
    {
        var data = await LoadJsonData<ModalidadTrabajo>("seed-modalidadtrabajo.json");
        if (data == null) return;

        foreach (var item in data)
        {
            if (!await _context.ModalidadesTrabajos.AnyAsync(m => m.NombreModalidad == item.NombreModalidad))
            {
                _context.ModalidadesTrabajos.Add(item);
            }
        }
        await _context.SaveChangesAsync();
    }

    private async Task SeedTipoContratoAsync()
    {
        var data = await LoadJsonData<TipoContrato>("seed-tipocontrato.json");
        if (data == null) return;

        foreach (var item in data)
        {
            if (!await _context.TiposContratos.AnyAsync(t => t.NombreTipo == item.NombreTipo))
            {
                _context.TiposContratos.Add(item);
            }
        }
        await _context.SaveChangesAsync();
    }

    private async Task SeedTipoFormacionAsync()
    {
        var data = await LoadJsonData<TipoFormacion>("seed-tipoformacion.json");
        if (data == null) return;

        foreach (var item in data)
        {
            if (!await _context.TiposFormacions.AnyAsync(t => t.NombreTipoFormacion == item.NombreTipoFormacion))
            {
                _context.TiposFormacions.Add(item);
            }
        }
        await _context.SaveChangesAsync();
    }

    private async Task SeedEmpresasAsync()
    {
        var data = await LoadJsonData<Empresa>("seed-empresa.json");
        if (data == null) return;

        foreach (var item in data)
        {
            // Verificación única por RUC
            if (!await _context.Empresas.AnyAsync(e => e.Ruc == item.Ruc))
            {
                _context.Empresas.Add(item);
            }
        }
        await _context.SaveChangesAsync();
    }

    private async Task SeedPersonasAsync()
    {
        var data = await LoadJsonData<Persona>("seed-persona.json");
        if (data == null) return;

        foreach (var item in data)
        {
            // Verificación única por DocumentoIdentidad
            if (!await _context.Personas.AnyAsync(p => p.DocumentoIdentidad == item.DocumentoIdentidad))
            {
                _context.Personas.Add(item);
            }
        }
        await _context.SaveChangesAsync();
    }

    private async Task SeedUsersAsync()
    {
        // Nota: El JSON de usuarios debe incluir una propiedad "Password" para poder crearlos,
        // aunque esa propiedad no esté en el modelo ApplicationUser.
        // Usaremos una clase DTO auxiliar interna para deserializar.

        var usersData = await LoadJsonData<UserSeedDto>("seed-users.json");
        if (usersData == null) return;

        foreach (var userDto in usersData)
        {
            if (await _userManager.FindByEmailAsync(userDto.Email!) == null)
            {
                var user = new ApplicationUser
                {
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    EmailConfirmed = true,
                    PhoneNumber = userDto.PhoneNumber
                    // Aquí podrías vincular IDs si fuera necesario, ej: Auditorias = ...
                };

                // Crear usuario con password
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"Usuario creado: {user.UserName}");

                    // Asignar Rol si viene en el JSON
                    if (!string.IsNullOrEmpty(userDto.Role))
                    {
                        if (await _roleManager.RoleExistsAsync(userDto.Role))
                        {
                            await _userManager.AddToRoleAsync(user, userDto.Role);
                        }
                    }
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError($"Error creando usuario {user.UserName}: {errors}");
                }
            }
        }
    }

    // DTO Interno para mapear el JSON de usuarios que incluye Password y Rol
    private class UserSeedDto
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!; // Importante para el CreateAsync
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; } // Nombre del rol a asignar
    }
}