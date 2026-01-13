using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data.Models;

namespace SiseApi.Data;

public partial class SiseDbContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
{
    public SiseDbContext(DbContextOptions<SiseDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrativo> Administrativo { get; set; }

    public virtual DbSet<Auditoria> Auditoria { get; set; }

    public virtual DbSet<CargoAdministrativo> CargoAdministrativo { get; set; }

    public virtual DbSet<Carrera> Carrera { get; set; }

    public virtual DbSet<Egresado> Egresado { get; set; }

    public virtual DbSet<Empresa> Empresa { get; set; }

    public virtual DbSet<Escuela> Escuela { get; set; }

    public virtual DbSet<ExperienciaLaboral> ExperienciaLaboral { get; set; }

    public virtual DbSet<Facultad> Facultad { get; set; }

    public virtual DbSet<FormacionComplementaria> FormacionComplementaria { get; set; }

    public virtual DbSet<ModalidadTrabajo> ModalidadTrabajo { get; set; }

    public virtual DbSet<OfertaLaboral> OfertaLaboral { get; set; }

    public virtual DbSet<Persona> Persona { get; set; }

    public virtual DbSet<Postulacion> Postulacion { get; set; }

    public virtual DbSet<Representante> Representante { get; set; }

    public virtual DbSet<TipoContrato> TipoContrato { get; set; }

    public virtual DbSet<TipoDocumento> TipoDocumento { get; set; }

    public virtual DbSet<TipoFormacion> TipoFormacion { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Administrativo>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue("Activo");

            entity.HasOne(d => d.IdCargoAdministrativoNavigation).WithMany(p => p.Administrativo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Administrativo_Cargo");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Administrativo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Administrativo_Persona");

                    });

        modelBuilder.Entity<Auditoria>(entity =>
        {
            entity.Property(e => e.FechaHora).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<CargoAdministrativo>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue("Activo");
        });

        modelBuilder.Entity<Carrera>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue("Activo");

            entity.HasOne(d => d.IdEscuelaNavigation).WithMany(p => p.Carrera)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Carrera_Escuela");
        });

        modelBuilder.Entity<Egresado>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue("Buscando Trabajo");

            entity.HasOne(d => d.IdCarreraNavigation).WithMany(p => p.Egresado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Egresado_Carrera");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Egresado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Egresado_Persona");

        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue("Registrada");
            entity.Property(e => e.Ruc).IsFixedLength();
        });

        modelBuilder.Entity<Escuela>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue("Activo");

            entity.HasOne(d => d.IdFacultadNavigation).WithMany(p => p.Escuela)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Escuela_Facultad");
        });

        modelBuilder.Entity<ExperienciaLaboral>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue("Validado");

            entity.HasOne(d => d.IdEgresadoNavigation).WithMany(p => p.ExperienciaLaboral).HasConstraintName("FK_ExperienciaLaboral_Egresado");

            entity.HasOne(d => d.IdEmpresaRegistradaNavigation).WithMany(p => p.ExperienciaLaboral).HasConstraintName("FK_ExperienciaLaboral_Empresa");
        });

        modelBuilder.Entity<Facultad>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue("Activo");
        });

        modelBuilder.Entity<FormacionComplementaria>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue("Validado");

            entity.HasOne(d => d.IdEgresadoNavigation).WithMany(p => p.FormacionComplementaria).HasConstraintName("FK_Formacion_Egresado");

            entity.HasOne(d => d.IdTipoFormacionNavigation).WithMany(p => p.FormacionComplementaria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Formacion_Tipo");
        });

        modelBuilder.Entity<ModalidadTrabajo>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue("Activo");
        });

        modelBuilder.Entity<OfertaLaboral>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue("Activo");
            entity.Property(e => e.FechaPublicacion).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdEgresadoGanadorNavigation).WithMany(p => p.OfertaLaboral).HasConstraintName("FK_OfertaLaboral_Ganador");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.OfertaLaboral)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfertaLaboral_Empresa");

            entity.HasOne(d => d.IdModalidadTrabajoNavigation).WithMany(p => p.OfertaLaboral)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfertaLaboral_Modalidad");

            entity.HasOne(d => d.IdTipoContratoNavigation).WithMany(p => p.OfertaLaboral)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfertaLaboral_TipoContrato");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue("Activo");

            entity.HasOne(d => d.IdTipoDocumentoNavigation).WithMany(p => p.Persona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Persona_TipoDocumento");
        });

        modelBuilder.Entity<Postulacion>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaPostulacion).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdEgresadoNavigation).WithMany(p => p.Postulacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Postulacion_Egresado");

            entity.HasOne(d => d.IdOfertaNavigation).WithMany(p => p.Postulacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Postulacion_Oferta");

            entity.HasOne(d => d.IdRepresentanteEvaluadorNavigation).WithMany(p => p.Postulacion).HasConstraintName("FK_Postulacion_Representante");
        });

        modelBuilder.Entity<Representante>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue("Activo");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Representante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Representante_Empresa");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Representante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Representante_Persona");

        });

        modelBuilder.Entity<TipoContrato>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue("Activo");
        });

        modelBuilder.Entity<TipoDocumento>(entity =>
        {
            entity.HasKey(e => e.IdTipoDocumento).HasName("PK__TipoDocu__61FDF9F5CF4B6FA7");

            entity.Property(e => e.Estado).HasDefaultValue("Activo");
        });

        modelBuilder.Entity<TipoFormacion>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue("Activo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
