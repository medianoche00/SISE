using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SiseApi.Data.Models;
using System;
using System.Collections.Generic;

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

    public virtual DbSet<Departamento> Departamento { get; set; }

    public virtual DbSet<Direccion> Direccion { get; set; }

    public virtual DbSet<Distrito> Distrito { get; set; }

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

    public virtual DbSet<Provincia> Provincia { get; set; }

    public virtual DbSet<Representante> Representante { get; set; }

    public virtual DbSet<TipoContrato> TipoContrato { get; set; }

    public virtual DbSet<TipoDocumento> TipoDocumento { get; set; }

    public virtual DbSet<TipoFormacion> TipoFormacion { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<IdentityUser<int>>(entity =>
        {
            entity.ToTable(name: "AspNetUsers", table => table.HasTrigger("TRG_Audit_AspNetUser"));
        });
        modelBuilder.Entity<Administrativo>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TRG_Audit_Administrativo"));

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
            entity.Property(e => e.FechaCambio).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UsuarioDb).HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<CargoAdministrativo>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TRG_Audit_CargoAdministrativo"));

            entity.Property(e => e.Estado).HasDefaultValue("Activo");
        });

        modelBuilder.Entity<Carrera>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TRG_Audit_Carrera"));

            entity.Property(e => e.Estado).HasDefaultValue("Activo");

            entity.HasOne(d => d.IdEscuelaNavigation).WithMany(p => p.Carrera)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Carrera_Escuela");
        });

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TRG_Audit_Departamento"));

            entity.Property(e => e.Estado).HasDefaultValue("Activo");
        });

        modelBuilder.Entity<Direccion>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TRG_Audit_Direccion"));

            entity.Property(e => e.Estado).HasDefaultValue("Activo");
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdDistritoNavigation).WithMany(p => p.Direccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Direccion_Distrito");
        });

        modelBuilder.Entity<Distrito>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TRG_Audit_Distrito"));

            entity.Property(e => e.Estado).HasDefaultValue("Activo");

            entity.HasOne(d => d.IdProvinciaNavigation).WithMany(p => p.Distrito)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Distrito_Provincia");
        });

        modelBuilder.Entity<Egresado>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TRG_Audit_Egresado"));

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
            entity.ToTable(tb => tb.HasTrigger("TRG_Audit_Empresa"));

            entity.Property(e => e.Estado).HasDefaultValue("Registrada");
            entity.Property(e => e.Ruc).IsFixedLength();

            entity.HasOne(d => d.IdDireccionNavigation).WithMany(p => p.Empresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Empresa_Direccion");
        });

        modelBuilder.Entity<Escuela>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TRG_Audit_Escuela"));

            entity.Property(e => e.Estado).HasDefaultValue("Activo");

            entity.HasOne(d => d.IdFacultadNavigation).WithMany(p => p.Escuela)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Escuela_Facultad");
        });

        modelBuilder.Entity<ExperienciaLaboral>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TRG_Audit_ExperienciaLaboral"));

            entity.Property(e => e.Estado).HasDefaultValue("Validado");

            entity.HasOne(d => d.IdEgresadoNavigation).WithMany(p => p.ExperienciaLaboral).HasConstraintName("FK_Experiencia_Egresado_Cascade");

            entity.HasOne(d => d.IdEmpresaRegistradaNavigation).WithMany(p => p.ExperienciaLaboral).HasConstraintName("FK_ExperienciaLaboral_Empresa");
        });

        modelBuilder.Entity<Facultad>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TRG_Audit_Facultad"));

            entity.Property(e => e.Estado).HasDefaultValue("Activo");
        });

        modelBuilder.Entity<FormacionComplementaria>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TRG_Audit_FormacionComplementaria"));

            entity.Property(e => e.Estado).HasDefaultValue("Validado");

            entity.HasOne(d => d.IdEgresadoNavigation).WithMany(p => p.FormacionComplementaria).HasConstraintName("FK_Formacion_Egresado_Cascade");

            entity.HasOne(d => d.IdTipoFormacionNavigation).WithMany(p => p.FormacionComplementaria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Formacion_Tipo");
        });

        modelBuilder.Entity<ModalidadTrabajo>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TRG_Audit_ModalidadTrabajo"));

            entity.Property(e => e.Estado).HasDefaultValue("Activo");
        });

        modelBuilder.Entity<OfertaLaboral>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TRG_Audit_OfertaLaboral"));

            entity.Property(e => e.Estado).HasDefaultValue("Activa");
            entity.Property(e => e.FechaPublicacion).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdDireccionNavigation).WithMany(p => p.OfertaLaboral)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfertaLaboral_Direccion");

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
            entity.ToTable(tb => tb.HasTrigger("TRG_Audit_Persona"));

            entity.Property(e => e.Estado).HasDefaultValue("Activo");

            entity.HasOne(d => d.IdDireccionNavigation).WithMany(p => p.Persona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Persona_Direccion");

            entity.HasOne(d => d.IdTipoDocumentoNavigation).WithMany(p => p.Persona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Persona_TipoDocumento");
        });

        modelBuilder.Entity<Postulacion>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TRG_Audit_Postulacion"));

            entity.Property(e => e.Estado).HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaPostulacion).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdEgresadoNavigation).WithMany(p => p.Postulacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Postulacion_Egresado");

            entity.HasOne(d => d.IdOfertaNavigation).WithMany(p => p.Postulacion).HasConstraintName("FK_Postulacion_Oferta_Cascade");

            entity.HasOne(d => d.IdRepresentanteEvaluadorNavigation).WithMany(p => p.Postulacion).HasConstraintName("FK_Postulacion_Representante");
        });

        modelBuilder.Entity<Provincia>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TRG_Audit_Provincia"));

            entity.Property(e => e.Estado).HasDefaultValue("Activo");

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Provincia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Provincia_Departamento");
        });

        modelBuilder.Entity<Representante>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TRG_Audit_Representante"));

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
            entity.ToTable(tb => tb.HasTrigger("TRG_Audit_TipoContrato"));

            entity.Property(e => e.Estado).HasDefaultValue("Activo");
        });

        modelBuilder.Entity<TipoDocumento>(entity =>
        {
            entity.HasKey(e => e.IdTipoDocumento).HasName("PK__TipoDocu__61FDF9F5633C810D");

            entity.ToTable(tb => tb.HasTrigger("TRG_Audit_TipoDocumento"));

            entity.Property(e => e.Estado).HasDefaultValue("Activo");
        });

        modelBuilder.Entity<TipoFormacion>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TRG_Audit_TipoFormacion"));

            entity.Property(e => e.Estado).HasDefaultValue("Activo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
