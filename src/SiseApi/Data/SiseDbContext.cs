using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SiseApi.Models; // Asegurate que ApplicationUser esté aquí o ajusta el using

namespace SiseApi.Data;

// Cambia la herencia para que ApplicationUser herede de IdentityUser<int>,
// y SiseDbContext herede de IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
public class SiseDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public SiseDbContext(DbContextOptions<SiseDbContext> options) : base(options)
    {
    }

    // --- TABLAS DE NEGOCIO (SE MANTIENEN) ---
    public virtual DbSet<Auditoria> Auditoria { get; set; }
    public virtual DbSet<Carrera> Carreras { get; set; }
    public virtual DbSet<Egresado> Egresados { get; set; }
    public virtual DbSet<Empresa> Empresas { get; set; }
    public virtual DbSet<Escuela> Escuelas { get; set; }
    public virtual DbSet<ExperienciaLaboral> ExperienciasLaborales { get; set; }
    public virtual DbSet<Facultad> Facultades { get; set; }
    public virtual DbSet<FormacionComplementaria> FormacionesComplementarias { get; set; }
    public virtual DbSet<ModalidadTrabajo> ModalidadesTrabajos { get; set; }
    public virtual DbSet<OfertaLaboral> OfertasLaborales { get; set; }

    // Aquí está el vínculo principal con el negocio
    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Representante> Representantes { get; set; }
    public virtual DbSet<TipoContrato> TiposContratos { get; set; }
    public virtual DbSet<TipoFormacion> TiposFormacions { get; set; }
    public virtual DbSet<VwOfertasDisponible> VwOfertasDisponibles { get; set; }

    // --- TABLAS ELIMINADAS (IDENTITY LAS REEMPLAZA) ---
    // Usuario, Rol, RolPermiso, Permiso -> BORRADOS

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 2. IMPORTANTE: Esta línea configura todas las tablas de Identity (AspNetUsers, AspNetRoles, etc.)
        base.OnModelCreating(modelBuilder);

        // Opcional: Si quieres cambiar el nombre de las tablas de Identity para que no sean "AspNetUsers"
        //modelBuilder.Entity<ApplicationUser>().ToTable("Usuarios_Identity");
        //modelBuilder.Entity <ApplicationRole>().ToTable("Roles_Identity");

        modelBuilder.Entity<Auditoria>(entity =>
        {
            entity.HasKey(e => e.IdAuditoria);
            entity.Property(e => e.FechaHora).HasDefaultValueSql("(getdate())");

            // Configuración de la relación
            entity.HasOne(d => d.Usuario)           // Propiedad de navegación en Auditoria
                  .WithMany(p => p.Auditorias)      // Propiedad de colección en ApplicationUser
                  .HasForeignKey(d => d.IdUsuario)  // Clave foránea en Auditoria (int)
                  .HasConstraintName("FK_Auditoria_Usuario_Identity"); // Nombre limpio para la FK
        });

        modelBuilder.Entity<Carrera>(entity =>
        {
            entity.HasKey(e => e.IdCarrera);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.HasOne(d => d.IdEscuelaNavigation).WithMany(p => p.Carreras)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Carrera_Escuela");
        });

        modelBuilder.Entity<Egresado>(entity =>
        {
            entity.HasKey(e => e.IdEgresado);
            entity.Property(e => e.Estado).HasDefaultValue(true);

            entity.HasOne(d => d.Carrera).WithMany(p => p.Egresados)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Egresado_Carrera");

            entity.HasOne(d => d.Persona).WithMany(p => p.Egresados)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Egresado_Persona");

            // ELIMINADO: La relación directa con "Usuario" antiguo.
            // Ahora se llega al usuario a través de Persona.
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.IdEmpresa);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.Ruc).IsFixedLength();
        });

        // ... (Escuela, Experiencia, Facultad, Formacion se mantienen igual pero sin referencias a tablas viejas) ...

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.IdPersona);

            // Si Persona tiene una columna IdUsuario tipo int:
            entity.HasOne(p => p.Usuario)
                  .WithOne(u => u.Persona)
                  .HasForeignKey<Persona>(p => p.IdUsuario); // Asegúrate que Persona.UsuarioId sea int
        });

        modelBuilder.Entity<Representante>(entity =>
        {
            entity.HasKey(e => e.IdRepresentante);
            entity.Property(e => e.Estado).HasDefaultValue(true);

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Representantes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Representante_Empresa");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Representantes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Representante_Persona");

            // ELIMINADO: Relación directa con Usuario antiguo.
        });

        // Configuración de vista (se mantiene igual)
        modelBuilder.Entity<VwOfertasDisponible>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("vwOfertasDisponibles");
        });
    }
}