using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data.Models;

namespace SiseApi.Data
{
    // Heredamos de IdentityDbContext especificando nuestro User, Role y el tipo de PK (int)
    public class SiseDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public SiseDbContext(DbContextOptions<SiseDbContext> options) : base(options)
        {
        }

        // --- DbSets de Negocio ---
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
        public virtual DbSet<Persona> Personas { get; set; }
        public virtual DbSet<Representante> Representantes { get; set; }
        public virtual DbSet<TipoContrato> TiposContratos { get; set; }
        public virtual DbSet<TipoFormacion> TiposFormacions { get; set; }
        public virtual DbSet<VwOfertasDisponible> VwOfertasDisponibles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. CRÍTICO: Configura todas las tablas de Identity (Users, Roles, Claims, etc.)
            base.OnModelCreating(modelBuilder);

            // 2. Configuraciones de tablas
            modelBuilder.Entity<Auditoria>(entity =>
            {
                entity.HasKey(e => e.IdAuditoria);
                entity.Property(e => e.FechaHora).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Usuario)
                      .WithMany(p => p.Auditorias)
                      .HasForeignKey(d => d.IdUsuario)
                      .HasConstraintName("FK_Auditoria_Usuario_Identity");
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

                // Relación con Carrera
                entity.HasOne(d => d.Carrera).WithMany(p => p.Egresados)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Egresado_Carrera");

                // Relación con Persona
                entity.HasOne(d => d.Persona).WithMany(p => p.Egresados)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Egresado_Persona");

                // Relación con Usuario (Identity)
                // Aquí es donde se une la cuenta de acceso con el perfil de egresado
                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Egresados)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Egresado_Usuario");
            });

            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.HasKey(e => e.IdEmpresa);
                entity.Property(e => e.Estado).HasDefaultValue(true);
                entity.Property(e => e.Ruc).IsFixedLength();
            });

            modelBuilder.Entity<Escuela>(entity =>
            {
                entity.HasKey(e => e.IdEscuela).HasName("PK__Escuela__9F67B289F35514A8");
                entity.Property(e => e.Estado).HasDefaultValue(true);
                entity.HasOne(d => d.IdFacultadNavigation).WithMany(p => p.Escuelas)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Escuela__idFacul__06CD04F7");
            });

            modelBuilder.Entity<ExperienciaLaboral>(entity =>
            {
                entity.HasKey(e => e.IdExperiencia).HasName("PK__Experien__77DCF2941080D70A");
                entity.Property(e => e.Estado).HasDefaultValue(true);
                entity.HasOne(d => d.IdEgresadoNavigation).WithMany(p => p.ExperienciaLaborals)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Experienc__idEgr__07C12930");
            });

            modelBuilder.Entity<Facultad>(entity =>
            {
                entity.HasKey(e => e.IdFacultad).HasName("PK__Facultad__B57E5B202D9641D0");
                entity.Property(e => e.Estado).HasDefaultValue(true);
            });

            modelBuilder.Entity<FormacionComplementaria>(entity =>
            {
                entity.HasKey(e => e.IdFormacion).HasName("PK__Formacio__9DE85F3DDE9E96E9");
                entity.Property(e => e.Estado).HasDefaultValue(true);
                entity.HasOne(d => d.IdEgresadoNavigation).WithMany(p => p.FormacionComplementaria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Formacion__idEgr__08B54D69");
                entity.HasOne(d => d.IdTipoFormacionNavigation).WithMany(p => p.FormacionComplementaria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Formacion__idTip__09A971A2");
            });

            modelBuilder.Entity<ModalidadTrabajo>(entity =>
            {
                entity.HasKey(e => e.IdModalidadTrabajo).HasName("PK__Modalida__82081B5D073BB510");
                entity.Property(e => e.Estado).HasDefaultValue(true);
            });

            modelBuilder.Entity<OfertaLaboral>(entity =>
            {
                entity.HasKey(e => e.IdOferta).HasName("PK__OfertaLa__05A1245E53519920");
                entity.Property(e => e.Estado).HasDefaultValue(true);
                entity.HasOne(d => d.IdEgresadoGanadorNavigation).WithMany(p => p.OfertaLaborals)
                      .HasConstraintName("FK__OfertaLab__idEgr__0A9D95DB");
                entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.OfertaLaborals)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OfertaLab__idEmp__0B91BA14");
                entity.HasOne(d => d.IdModalidadTrabajoNavigation).WithMany(p => p.OfertaLaborals)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OfertaLab__idMod__0C85DE4D");
                entity.HasOne(d => d.IdTipoContratoNavigation).WithMany(p => p.OfertaLaborals)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OfertaLab__idTip__0D7A0286");
            });

            // --- CORRECCIÓN CLAVE EN PERSONA ---
            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.IdPersona);
                // NOTA IMPORTANTE: Aquí NO se define ninguna relación con Usuario.
                // Si la clase C# 'ApplicationUser' está limpia, EF no creará 'IdUsuario' en esta tabla.
            });

            // --- CORRECCIÓN EN REPRESENTANTE ---
            modelBuilder.Entity<Representante>(entity =>
            {
                entity.HasKey(e => e.IdRepresentante);
                entity.Property(e => e.Estado).HasDefaultValue(true);

                // Relación con Empresa
                entity.HasOne(d => d.Empresa)
                    .WithMany(p => p.Representantes)
                    .HasForeignKey(d => d.IdEmpresa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Representante_Empresa");

                // Relación con Persona
                entity.HasOne(d => d.Persona)
                    .WithMany(p => p.Representantes)
                    .HasForeignKey(d => d.IdPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Representante_Persona");

                // Relación con Usuario (Identity)
                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Representantes)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Representante_Usuario");
            });

            modelBuilder.Entity<TipoContrato>(entity =>
            {
                // Se asume que tiene PK definida en la clase o por convención, 
                // pero si quieres ser explícito agrégalo aquí.
            });

            modelBuilder.Entity<TipoFormacion>(entity =>
            {
                // Igual que arriba.
            });

            modelBuilder.Entity<VwOfertasDisponible>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vwOfertasDisponibles");
            });
        }
    }
}