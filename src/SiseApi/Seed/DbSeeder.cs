using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Data.Models;
using SiseApi.Seed;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

// Asegúrate de importar los namespaces de tus Modelos y tu Contexto
// using TuProyecto.Models;
// using TuProyecto.Data;

public class DbSeeder : IDbSeeder
{
    private readonly SiseDbContext _context;
    private readonly UserManager<IdentityUser<int>> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<DbSeeder> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public DbSeeder(
        SiseDbContext context,
        UserManager<IdentityUser<int>> userManager,
        RoleManager<IdentityRole<int>> roleManager,
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

        // Añadir conversor para DateOnly (formato ISO yyyy-MM-dd)
        _jsonOptions.Converters.Add(new DateOnlyJsonConverter());
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // 1. Tablas Maestras / Catálogos (Sin dependencias)
            await SeedCargosAdministrativosAsync();
            await SeedRolesAsync();
            await SeedFacultadesAsync();
            await SeedModalidadTrabajoAsync();
            await SeedTipoContratoAsync();
            await SeedTipoFormacionAsync();

            // 2. Tablas con Dependencias de Nivel 1
            await SeedEscuelasAsync(); // Depende de Facultad
            await SeedEmpresaAsync();
            await SeedOfertasAsync(); // Depende de Empresa, ModalidadTrabajo, TipoContrato
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

    private async Task SeedCargosAdministrativosAsync()
    {
        var cargos = await LoadJsonData<CargoAdministrativo>("seed-cargos.json");
        if (cargos == null) return;
        
        foreach (var cargo in cargos)
        {
            if (!await _context.CargoAdministrativo.AnyAsync(c => c.NombreCargo == cargo.NombreCargo))
            {
                _context.CargoAdministrativo.Add(cargo);
            }
        }
        _context.SaveChanges();
    }

    private async Task SeedRolesAsync()
    {
        // Archivo esperado: seed-roles.json
        // Estructura JSON sugerida: [{"Name": "Admin"}, {"Name": "Egresado"}, {"Name": "Empresa"}]
        var roles = await LoadJsonData<IdentityRole<int>>("seed-roles.json");
        if (roles == null) return;

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role.Name!))
            {
                await _roleManager.CreateAsync(new IdentityRole<int>(role.Name!));
                _logger.LogInformation($"Rol creado: {role.Name}");
            }
        }
    }

    private async Task SeedFacultadesAsync()
    {
        if (await _context.Facultad.AnyAsync()) return; // Evitar duplicados masivos

        var data = await LoadJsonData<Facultad>("seed-facultad.json");
        if (data != null)
        {
            await _context.Facultad.AddRangeAsync(data);
            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedEscuelasAsync()
    {
        if (await _context.Escuela.AnyAsync()) return;

        var data = await LoadJsonData<Escuela>("seed-escuela.json");
        // Asegurarse de que las Facultades existan por ID o hacer un lookup aquí si el JSON no tiene IDs correctos
        if (data != null)
        {
            await _context.Escuela.AddRangeAsync(data);
            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedCarrerasAsync()
    {
        if (await _context.Carrera.AnyAsync()) return;

        var data = await LoadJsonData<Carrera>("seed-carrera.json");
        if (data != null)
        {
            await _context.Carrera.AddRangeAsync(data);
            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedModalidadTrabajoAsync()
    {
        var data = await LoadJsonData<ModalidadTrabajo>("seed-modalidadtrabajo.json");
        if (data == null) return;

        foreach (var item in data)
        {
            if (!await _context.ModalidadTrabajo.AnyAsync(m => m.NombreModalidad == item.NombreModalidad))
            {
                _context.ModalidadTrabajo.Add(item);
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
            if (!await _context.TipoContrato.AnyAsync(t => t.NombreTipo == item.NombreTipo))
            {
                _context.TipoContrato.Add(item);
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
            if (!await _context.TipoFormacion.AnyAsync(t => t.NombreTipoFormacion == item.NombreTipoFormacion))
            {
                _context.TipoFormacion.Add(item);
            }
        }
        await _context.SaveChangesAsync();
    }

    private async Task SeedEmpresaAsync()
    {
        var data = await LoadJsonData<Empresa>("seed-empresa.json");
        if (data == null) return;

        foreach (var item in data)
        {
            // Verificación única por RUC
            if (!await _context.Empresa.AnyAsync(e => e.Ruc == item.Ruc))
            {
                _context.Empresa.Add(item);
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
            if (!await _context.Persona.AnyAsync(p => p.DocumentoIdentidad == item.DocumentoIdentidad))
            {
                _context.Persona.Add(item);
            }
        }
        await _context.SaveChangesAsync();
    }

    private async Task SeedOfertasAsync()
    {
        if (await _context.OfertaLaboral.AnyAsync()) return;

        var data = await LoadJsonData<OfertaLaboral>("seed-ofertas.json");
        if (data == null) return;

        foreach (var item in data)
        {
            // Evitar duplicados: título + empresa + fechaPublicacion
            if (!await _context.OfertaLaboral.AnyAsync(o =>
                o.Titulo == item.Titulo && o.IdEmpresa == item.IdEmpresa && o.FechaPublicacion == item.FechaPublicacion))
            {
                // Asegurar referencias mínimas: si la empresa no existe, saltar el registro
                if (!await _context.Empresa.AnyAsync(e => e.IdEmpresa == item.IdEmpresa))
                {
                    _logger.LogWarning($"Omitida oferta '{item.Titulo}': empresa id {item.IdEmpresa} no encontrada.");
                    continue;
                }

                // Opcional: validar que idTipoContrato e idModalidadTrabajo existan
                if (!await _context.TipoContrato.AnyAsync(t => t.IdTipoContrato == item.IdTipoContrato))
                {
                    _logger.LogWarning($"Omitida oferta '{item.Titulo}': tipo contrato id {item.IdTipoContrato} no encontrado.");
                    continue;
                }
                if (!await _context.ModalidadTrabajo.AnyAsync(m => m.IdModalidadTrabajo == item.IdModalidadTrabajo))
                {
                    _logger.LogWarning($"Omitida oferta '{item.Titulo}': modalidad trabajo id {item.IdModalidadTrabajo} no encontrado.");
                    continue;
                }

                _context.OfertaLaboral.Add(item);
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task SeedUsersAsync()
    {
        // Nota: El JSON de usuarios debe incluir una propiedad "Password" para poder crearlos,
        // aunque esa propiedad no esté en el modelo IdentityUser<int>.
        // Usaremos una clase DTO auxiliar interna para deserializar.

        var usersData = await LoadJsonData<UserSeedDto>("seed-users.json");
        if (usersData == null) return;

        foreach (var userDto in usersData)
        {
            if (await _userManager.FindByNameAsync(userDto.UserName!) == null)
            {
                var user = new IdentityUser<int>
                {
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    EmailConfirmed = true,
                    PhoneNumber = userDto.PhoneNumber
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

                    // Cargar datos en sus tablas de acuerdo al rol
                    if (userDto.Role == "Egresado")
                    {
                        await CreateEgresadoProfile(user, userDto);
                    }
                    else if (userDto.Role == "Representante")
                    {
                        await CreateRepresentanteProfile(user, userDto);
                    }
                    else if (userDto.Role == "Administrativo" || userDto.Role == "Administrador") 
                    {
                        await CreateAdministrativoProfile(user, userDto);
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

    private async Task CreateEgresadoProfile(IdentityUser<int> user, UserSeedDto dto)
    {
        // Buscar la persona por DNI (u otro documento)
        var persona = await _context.Persona
            .FirstOrDefaultAsync(p => p.DocumentoIdentidad == dto.DocumentoIdentidad);

        if (persona == null)
        {
            _logger.LogWarning($"No se encontró Persona con Doc {dto.DocumentoIdentidad} para el usuario {user.UserName}");
            return;
        }

        if (dto.CodigoUniversitario == null)
        {
            _logger.LogWarning($"El usuario {user.UserName} es un Egresado pero no tiene codigo universitario");
            return;
        }

        var egresado = new Egresado
        {
            IdUsuario = user.Id, // Vinculamos al ID del usuario recién creado
            IdPersona = persona.IdPersona, // Vinculamos a la persona existente
            IdCarrera = dto.IdCarrera ?? 1, // Valor por defecto o error si es nulo
            CodigoUniversitario = dto.CodigoUniversitario,
            AñoEgreso = dto.AnioEgreso ?? DateTime.Now.Year,
            Estado = "Buscando Trabajo"
        };

        _context.Egresado.Add(egresado);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Perfil Egresado creado para {user.UserName}");
    }

    private async Task CreateRepresentanteProfile(IdentityUser<int> user, UserSeedDto dto)
    {
        // 1. Buscar Persona
        var persona = await _context.Persona
            .FirstOrDefaultAsync(p => p.DocumentoIdentidad == dto.DocumentoIdentidad);

        // 2. Buscar Empresa (usando un campo único como RUC, nombre, o asumiendo que el JSON trae el ID)
        // Aquí asumo que la entidad Empresa tiene un campo RUC o similar.
        var empresa = await _context.Empresa
            .FirstOrDefaultAsync(e => e.Ruc == dto.RucEmpresa);

        if (persona == null || empresa == null)
        {
            _logger.LogWarning($"Faltan datos (Persona o Empresa) para crear representante {user.UserName}");
            return;
        }

        var representante = new Representante
        {
            IdUsuario = user.Id,
            IdPersona = persona.IdPersona,
            IdEmpresa = empresa.IdEmpresa, // ID de la empresa encontrada
            Cargo = dto.CargoEmpresa,
            Estado = "Activo"
        };

        _context.Representante.Add(representante);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Perfil Representante creado para {user.UserName}");
    }

    private async Task CreateAdministrativoProfile(IdentityUser<int> user, UserSeedDto dto)
    {
        // 1. Buscar Persona
        var persona = await _context.Persona
            .FirstOrDefaultAsync(p => p.DocumentoIdentidad == dto.DocumentoIdentidad);

        // 2. Buscar Cargo
        var cargo = await _context.CargoAdministrativo
            .FirstOrDefaultAsync(e => e.NombreCargo == dto.CargoAdministrativo);

        if (persona == null || cargo == null)
        {
            _logger.LogWarning($"Faltan datos (Persona o Cargo) para crear representante {user.UserName}");
            return;
        }

        var administrativo = new Administrativo
        {
            IdUsuario = user.Id,
            IdPersona = persona.IdPersona,
            IdCargoAdministrativo = cargo.IdCargoAdministrativo,
            Estado = "Activo"
        };

        _context.Administrativo.Add(administrativo);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Perfil Administrativo creado para {user.UserName}");
    }

    // DTO Interno para mapear el JSON de usuarios que incluye Password y Rol
    private class UserSeedDto
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; } // Nombre del rol a asignar
        public string? DocumentoIdentidad { get; set; }
        // Datos específicos para Egresado
        public string? CodigoUniversitario { get; set; }
        public int? IdCarrera { get; set; }
        public int? AnioEgreso { get; set; }
        // Datos específicos para Representante
        public string? RucEmpresa { get; set; }
        public string? CargoEmpresa { get; set; }
        // Datos específicos para Administrativo
        public string? CargoAdministrativo { get; set; }
    }

    // Conversor simple para DateOnly en formato ISO yyyy-MM-dd
    private class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string Format = "yyyy-MM-dd";
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var s = reader.GetString() ?? throw new JsonException("Expected string for DateOnly");
            if (DateOnly.TryParseExact(s, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var d))
                return d;
            if (DateOnly.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                return d;
            throw new JsonException($"No se pudo parsear la fecha DateOnly: {s}");
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
        }
    }
}