using Microsoft.AspNetCore.Identity; // NECESARIO
using Microsoft.EntityFrameworkCore;
using SiseApi.Data;
using SiseApi.Models;
using System.Text.Json;

namespace SiseApi.Seed
{
    public class DbSeeder : IDbSeeder
    {
        private readonly SiseDbContext _context;
        private readonly IHostEnvironment _env;
        private readonly ILogger<DbSeeder> _logger;

        // NUEVAS DEPENDENCIAS DE IDENTITY
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public DbSeeder(
            SiseDbContext context,
            IHostEnvironment env,
            ILogger<DbSeeder> logger,
            UserManager<ApplicationUser> userManager, // Inyectamos el gestor de usuarios
            RoleManager<ApplicationRole> roleManager)  // Inyectamos el gestor de roles
        {
            _context = context;
            _env = env;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            if (!_env.IsDevelopment())
            {
                _logger.LogInformation("Seeder omitido: No estamos en entorno de desarrollo.");
                return;
            }

            try
            {
                await SeedCatalogsAsync();       // Tablas simples (sin cambios)
                await SeedSecurityAsync();       // !!! CAMBIO PRINCIPAL (Identity)
                await SeedAcademicAsync();       // Tablas de negocio (ligeros ajustes)
                await SeedBusinessAsync();       // Tablas de negocio

                _logger.LogInformation("=== Proceso de Seed completado exitosamente ===");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico durante la ejecución del DbSeeder.");
                throw;
            }
        }

        private async Task SeedSecurityAsync()
        {
            // 1. ROLES (Usando RoleManager)
            var roles = LoadJson<RolSeedDto>("seed-rol.json"); // Usamos un DTO simple
            if (roles != null)
            {
                foreach (var rolDto in roles)
                {
                    // Verificamos si existe usando el Manager
                    if (!await _roleManager.RoleExistsAsync(rolDto.NombreRol))
                    {
                        var nuevoRol = new ApplicationRole(rolDto.NombreRol);
                        await _roleManager.CreateAsync(nuevoRol);
                    }
                }
                _logger.LogInformation("Roles de Identity verificados.");
            }

            // 2. USUARIOS (Usando UserManager)
            var usuariosDto = LoadJson<UsuarioSeedDto>("seed-usuario.json");

            if (usuariosDto != null)
            {
                foreach (var userDto in usuariosDto)
                {
                    // Buscamos por nombre de usuario
                    var usuarioExistente = await _userManager.FindByNameAsync(userDto.Usuario);

                    if (usuarioExistente == null)
                    {
                        // Creamos la instancia de ApplicationUser
                        var nuevoUsuario = new ApplicationUser
                        {
                            UserName = userDto.Usuario,
                            Email = userDto.Usuario + "@ejemplo.com", // Identity suele requerir Email
                            EmailConfirmed = true, // Auto-confirmamos para desarrollo
                            // Persona = new Persona { ... } // OPCIONAL: Podrías crear la Persona aquí mismo
                        };

                        // A. CREAR USUARIO Y HASHEAR PASSWORD
                        // CreateAsync toma el usuario y la clave en texto plano, y la encripta sola.
                        var resultado = await _userManager.CreateAsync(nuevoUsuario, userDto.ClavePlana);

                        if (resultado.Succeeded)
                        {
                            // B. ASIGNAR ROL
                            // Verificamos que el rol exista antes de asignarlo
                            if (await _roleManager.RoleExistsAsync(userDto.Rol))
                            {
                                await _userManager.AddToRoleAsync(nuevoUsuario, userDto.Rol);
                            }
                            else
                            {
                                _logger.LogWarning($"El rol '{userDto.Rol}' no existe. Usuario creado sin rol.");
                            }
                        }
                        else
                        {
                            var errores = string.Join(", ", resultado.Errors.Select(e => e.Description));
                            _logger.LogError($"Error al crear usuario {userDto.Usuario}: {errores}");
                        }
                    }
                }
                _logger.LogInformation("Usuarios de Identity verificados.");
            }
        }

        // --- MÉTODOS EXISTENTES (Se mantienen casi igual, solo verifica nombres de tablas) ---

        private async Task SeedCatalogsAsync()
        {
            // Modalidad Trabajo
            var modalidades = LoadJson<ModalidadTrabajo>("seed-modalidad.json");
            if (modalidades != null)
            {
                foreach (var item in modalidades)
                {
                    if (!await _context.ModalidadesTrabajos.AnyAsync(x => x.NombreModalidad == item.NombreModalidad))
                        await _context.ModalidadesTrabajos.AddAsync(item);
                }
                await _context.SaveChangesAsync();
            }

            // Tipo Contrato
            var contratos = LoadJson<TipoContrato>("seed-tipocontrato.json");
            if (contratos != null)
            {
                foreach (var item in contratos)
                {
                    if (!await _context.TiposContratos.AnyAsync(x => x.NombreTipo == item.NombreTipo))
                        await _context.TiposContratos.AddAsync(item);
                }
                await _context.SaveChangesAsync();
            }

            // Tipo Formacion
            var formaciones = LoadJson<TipoFormacion>("seed-tipoformacion.json");
            if (formaciones != null)
            {
                foreach (var item in formaciones)
                {
                    if (!await _context.TiposFormacions.AnyAsync(x => x.NombreTipoFormacion == item.NombreTipoFormacion))
                        await _context.TiposFormacions.AddAsync(item);
                }
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedAcademicAsync()
        {
            // Facultades
            var facultades = LoadJson<Facultad>("seed-facultad.json");
            if (facultades != null)
            {
                foreach (var item in facultades)
                {
                    if (!await _context.Facultades.AnyAsync(x => x.NombreFacultad == item.NombreFacultad))
                        await _context.Facultades.AddAsync(item);
                }
                await _context.SaveChangesAsync();
            }

            // Escuelas
            var escuelasDto = LoadJson<EscuelaSeedDto>("seed-escuela.json");
            if (escuelasDto != null)
            {
                var dbFacultades = await _context.Facultades.ToListAsync();
                foreach (var dto in escuelasDto)
                {
                    if (!await _context.Escuelas.AnyAsync(x => x.NombreEscuela == dto.NombreEscuela))
                    {
                        var facultad = dbFacultades.FirstOrDefault(f => f.NombreFacultad == dto.Facultad);
                        if (facultad != null)
                        {
                            await _context.Escuelas.AddAsync(new Escuela
                            {
                                NombreEscuela = dto.NombreEscuela,
                                IdFacultad = facultad.IdFacultad,
                                Estado = dto.Estado
                            });
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }

            // Carreras
            var carrerasDto = LoadJson<CarreraSeedDto>("seed-carrera.json");
            if (carrerasDto != null)
            {
                var dbEscuelas = await _context.Escuelas.ToListAsync();
                foreach (var dto in carrerasDto)
                {
                    if (!await _context.Carreras.AnyAsync(x => x.NombreCarrera == dto.NombreCarrera))
                    {
                        var escuela = dbEscuelas.FirstOrDefault(e => e.NombreEscuela == dto.Escuela);
                        if (escuela != null)
                        {
                            await _context.Carreras.AddAsync(new Carrera
                            {
                                NombreCarrera = dto.NombreCarrera,
                                IdEscuela = escuela.IdEscuela,
                                Estado = dto.Estado
                            });
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedBusinessAsync()
        {
            var empresas = LoadJson<Empresa>("seed-empresa.json");
            if (empresas != null)
            {
                foreach (var emp in empresas)
                {
                    if (!await _context.Empresas.AnyAsync(e => e.Ruc == emp.Ruc))
                        await _context.Empresas.AddAsync(emp);
                }
                await _context.SaveChangesAsync();
            }
        }

        // --- Helpers ---

        private List<T> LoadJson<T>(string fileName)
        {
            var path = Path.Combine(_env.ContentRootPath, "SeedData", fileName);
            if (!File.Exists(path)) return null;

            var json = File.ReadAllText(path);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<T>>(json, options);
        }

        // DTOs Internos para los JSON
        private class UsuarioSeedDto
        {
            public string Usuario { get; set; }
            public string ClavePlana { get; set; }
            public string Rol { get; set; }
            public bool Estado { get; set; }
        }

        // Necesitas un DTO para Rol porque tu JSON seguro tiene "NombreRol" 
        // y ApplicationRole usa "Name" internamente, aunque el constructor lo mapea.
        private class RolSeedDto
        {
            public string NombreRol { get; set; }
        }

        private class EscuelaSeedDto
        {
            public string NombreEscuela { get; set; }
            public string Facultad { get; set; }
            public bool Estado { get; set; }
        }

        private class CarreraSeedDto
        {
            public string NombreCarrera { get; set; }
            public string Escuela { get; set; }
            public bool Estado { get; set; }
        }
    }
}