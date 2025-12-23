using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public DbSeeder(SiseDbContext context, IHostEnvironment env, ILogger<DbSeeder> logger)
        {
            _context = context;
            _env = env;
            _logger = logger;
        }

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            // Solo ejecutar en Desarrollo para evitar accidentes en Prod, 
            // aunque la lógica es idempotente.
            if (!_env.IsDevelopment())
            {
                _logger.LogInformation("Seeder omitido: No estamos en entorno de desarrollo.");
                return;
            }

            try
            {
                // Usamos una transacción para asegurar integridad en bloques lógicos
                // Nota: EF Core soporta transacciones implícitas en SaveChanges, 
                // pero aquí agrupamos varias operaciones.

                await SeedCatalogsAsync();       // Modalidad, TipoContrato, TipoFormacion
                await SeedSecurityAsync();       // Roles y Usuarios
                await SeedAcademicAsync();       // Facultad -> Escuela -> Carrera
                await SeedBusinessAsync();       // Empresas    

                _logger.LogInformation("=== Proceso de Seed completado exitosamente ===");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico durante la ejecución del DbSeeder.");
                throw;
            }
        }

        private async Task SeedCatalogsAsync()
        {
            // 1. Modalidad Trabajo
            var modalidades = LoadJson<ModalidadTrabajo>("seed-modalidad.json");
            if (modalidades != null)
            {
                foreach (var item in modalidades)
                {
                    if (!await _context.ModalidadesTrabajos.AnyAsync(x => x.NombreModalidad == item.NombreModalidad))
                    {
                        await _context.ModalidadesTrabajos.AddAsync(item);
                    }
                }
                await _context.SaveChangesAsync();
                _logger.LogInformation("Catálogo ModalidadTrabajo verificado/sembrado.");
            }

            // 2. Tipo Contrato
            var contratos = LoadJson<TipoContrato>("seed-tipocontrato.json");
            if (contratos != null)
            {
                foreach (var item in contratos)
                {
                    if (!await _context.TiposContratos.AnyAsync(x => x.NombreTipo == item.NombreTipo))
                    {
                        await _context.TiposContratos.AddAsync(item);
                    }
                }
                await _context.SaveChangesAsync();
                _logger.LogInformation("Catálogo TipoContrato verificado/sembrado.");
            }

            // 3. NUEVO: Tipo de Formación
            var formaciones = LoadJson<TipoFormacion>("seed-tipoformacion.json");
            if (formaciones != null)
            {
                foreach (var item in formaciones)
                {
                    if (!await _context.TiposFormacions.AnyAsync(x => x.NombreTipoFormacion == item.NombreTipoFormacion))
                    {
                        await _context.TiposFormacions.AddAsync(item);
                    }
                }
                await _context.SaveChangesAsync();
                _logger.LogInformation("Catálogo TipoFormacion sembrado.");
            }
        }

        private async Task SeedAcademicAsync()
        {
            // 1. Facultades (Ya lo tenías, asegúrate que se cargue primero)
            var facultades = LoadJson<Facultad>("seed-facultad.json");
            if (facultades != null)
            {
                foreach (var item in facultades)
                {
                    if (!await _context.Facultades.AnyAsync(x => x.NombreFacultad == item.NombreFacultad))
                    {
                        await _context.Facultades.AddAsync(item);
                    }
                }
                await _context.SaveChangesAsync();
            }

            // 2. Escuelas (NUEVO: Lógica con búsqueda de FK)
            var escuelasDto = LoadJson<EscuelaSeedDto>("seed-escuela.json");
            if (escuelasDto != null)
            {
                // Traemos las facultades a memoria para evitar queries en bucle (optimización simple)
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
                                IdFacultad = facultad.IdFacultad, // Asignamos el ID real
                                Estado = dto.Estado
                            });
                        }
                        else
                        {
                            _logger.LogWarning($"Facultad '{dto.Facultad}' no encontrada para escuela '{dto.NombreEscuela}'.");
                        }
                    }
                }
                await _context.SaveChangesAsync();
                _logger.LogInformation("Escuelas sembradas.");
            }

            // 3. Carreras (NUEVO: Lógica con búsqueda de FK)
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
                                IdEscuela = escuela.IdEscuela, // Asignamos el ID real
                                Estado = dto.Estado
                            });
                        }
                        else
                        {
                            _logger.LogWarning($"Escuela '{dto.Escuela}' no encontrada para carrera '{dto.NombreCarrera}'.");
                        }
                    }
                }
                await _context.SaveChangesAsync();
                _logger.LogInformation("Carreras sembradas.");
            }
        }

        private async Task SeedSecurityAsync()
        {
            // 1. Roles
            var roles = LoadJson<Rol>("seed-rol.json");
            if (roles != null)
            {
                foreach (var rol in roles)
                {
                    if (!await _context.Roles.AnyAsync(r => r.NombreRol == rol.NombreRol))
                    {
                        await _context.Roles.AddAsync(rol);
                    }
                }
                await _context.SaveChangesAsync();
                _logger.LogInformation("Roles verificados/sembrado.");
            }

            // 2. Usuarios (Custom Logic)
            // Usamos una clase DTO temporal para leer el JSON porque el JSON tiene "ClavePlana" y "Rol" (nombre)
            // pero la entidad requiere "ClaveHash" y "IdRol".
            var usuariosDto = LoadJson<UsuarioSeedDto>("seed-usuario.json");

            if (usuariosDto != null)
            {
                foreach (var userDto in usuariosDto)
                {
                    if (!await _context.Usuarios.AnyAsync(u => u.Nomusuario == userDto.Usuario))
                    {
                        // Buscar el ID del rol por nombre
                        var rol = await _context.Roles.FirstOrDefaultAsync(r => r.NombreRol == userDto.Rol);
                        if (rol == null)
                        {
                            _logger.LogWarning($"No se encontró el rol '{userDto.Rol}' para el usuario '{userDto.Usuario}'. Omitiendo.");
                            continue;
                        }

                        var nuevoUsuario = new Usuario
                        {
                            Nomusuario = userDto.Usuario, // Asumiendo que EF generó 'Usuario1' por la colisión de nombres
                            IdRol = rol.IdRol,
                            Estado = userDto.Estado,
                            // IMPORTANTE: En un entorno real, usa BCrypt o Argon2
                            ClaveHash = FakeHashPassword(userDto.ClavePlana)
                        };

                        await _context.Usuarios.AddAsync(nuevoUsuario);
                    }
                }
                await _context.SaveChangesAsync();
                _logger.LogInformation("Usuarios verificados/sembrado.");
            }
        }
        private async Task SeedBusinessAsync()
        {
            var empresas = LoadJson<Empresa>("seed-empresa.json");
            if (empresas != null)
            {
                foreach (var emp in empresas)
                {
                    // Clave natural: RUC
                    if (!await _context.Empresas.AnyAsync(e => e.Ruc == emp.Ruc))
                    {
                        await _context.Empresas.AddAsync(emp);
                    }
                }
                await _context.SaveChangesAsync();
                _logger.LogInformation("Empresas sembradas.");
            }
        }

        // --- Helpers ---

        private List<T> LoadJson<T>(string fileName)
        {
            var path = Path.Combine(_env.ContentRootPath, "SeedData", fileName);
            if (!File.Exists(path))
            {
                _logger.LogWarning($"Archivo de seed no encontrado: {path}");
                return null;
            }

            var json = File.ReadAllText(path);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<T>>(json, options);
        }

        private string FakeHashPassword(string password)
        {
            // TODO: REEMPLAZAR ESTO con tu servicio de Hashing real (ej. BCrypt.HashPassword)
            // Esto es solo para que el campo no quede vacío en la demo.
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + "_SALT_DEMO"));
        }

        // DTO Interno para mapear el JSON de usuarios
        private class UsuarioSeedDto
        {
            public string Usuario { get; set; }
            public string ClavePlana { get; set; }
            public string Rol { get; set; } // Nombre del rol para buscar FK
            public bool Estado { get; set; }
        }

        private class EscuelaSeedDto
        {
            public string NombreEscuela { get; set; }
            public string Facultad { get; set; } // Nombre de la facultad padre
            public bool Estado { get; set; }
        }

        private class CarreraSeedDto
        {
            public string NombreCarrera { get; set; }
            public string Escuela { get; set; } // Nombre de la escuela padre
            public bool Estado { get; set; }
        }
    }
}