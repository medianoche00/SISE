using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SiseApi.Models;

namespace SiseApi.Data;

public partial class SiseDbContext : DbContext
{
    public SiseDbContext(DbContextOptions<SiseDbContext> options)
        : base(options)
    {
    }

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

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Representante> Representantes { get; set; }

    public virtual DbSet<Rol> Roles { get; set; }

    public virtual DbSet<RolPermiso> RolesPermisos { get; set; }

    public virtual DbSet<TipoContrato> TiposContratos { get; set; }

    public virtual DbSet<TipoFormacion> TiposFormacions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<VwOfertasDisponible> VwOfertasDisponibles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auditoria>(entity =>
        {
            entity.HasKey(e => e.IdAuditoria).HasName("PK__Auditori__F1F3070140F5D46E");

            entity.Property(e => e.FechaHora).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Auditoria).HasConstraintName("FK__Auditoria__idUsu__02084FDA");
        });

        modelBuilder.Entity<Carrera>(entity =>
        {
            entity.HasKey(e => e.IdCarrera).HasName("PK__Carrera__7B19E7911380B0F6");

            entity.Property(e => e.Estado).HasDefaultValue(true);

            entity.HasOne(d => d.IdEscuelaNavigation).WithMany(p => p.Carreras)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Carrera__idEscue__02FC7413");
        });

        modelBuilder.Entity<Egresado>(entity =>
        {
            entity.HasKey(e => e.IdEgresado).HasName("PK__Egresado__D0DFB23B87382E2F");

            entity.Property(e => e.Estado).HasDefaultValue(true);

            entity.HasOne(d => d.IdCarreraNavigation).WithMany(p => p.Egresados)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Egresado__idCarr__03F0984C");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Egresados)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Egresado__idPers__04E4BC85");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Egresados)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Egresado__idUsua__05D8E0BE");
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.IdEmpresa).HasName("PK__Empresa__75D2CED4E8E285E8");

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

            entity.HasOne(d => d.IdEgresadoGanadorNavigation).WithMany(p => p.OfertaLaborals).HasConstraintName("FK__OfertaLab__idEgr__0A9D95DB");

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

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermiso).HasName("PK__Permiso__06A58486BD3A1838");

            entity.Property(e => e.Estado).HasDefaultValue(true);
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.IdPersona).HasName("PK__Persona__A47881413D7D6FCC");

            entity.Property(e => e.Dni).IsFixedLength();
            entity.Property(e => e.Estado).HasDefaultValue(true);
        });

        modelBuilder.Entity<Representante>(entity =>
        {
            entity.HasKey(e => e.IdRepresentante).HasName("PK__Represen__119773E39A1935AE");

            entity.Property(e => e.Estado).HasDefaultValue(true);

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Representantes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Represent__idEmp__0E6E26BF");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Representantes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Represent__idPer__0F624AF8");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Representantes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Represent__idUsu__10566F31");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__3C872F76A35E572A");

            entity.Property(e => e.Estado).HasDefaultValue(true);
        });

        modelBuilder.Entity<RolPermiso>(entity =>
        {
            entity.HasKey(e => e.IdRolPermiso).HasName("PK__RolPermi__461A1485D315AE4E");

            entity.Property(e => e.Estado).HasDefaultValue(true);

            entity.HasOne(d => d.IdPermisoNavigation).WithMany(p => p.RolPermisos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RolPermis__idPer__114A936A");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.RolPermisos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RolPermis__idRol__123EB7A3");
        });

        modelBuilder.Entity<TipoContrato>(entity =>
        {
            entity.HasKey(e => e.IdTipoContrato).HasName("PK__TipoCont__2FE56AF522569F57");

            entity.Property(e => e.Estado).HasDefaultValue(true);
        });

        modelBuilder.Entity<TipoFormacion>(entity =>
        {
            entity.HasKey(e => e.IdTipoFormacion).HasName("PK__TipoForm__3BF8417D95403AA8");

            entity.Property(e => e.Estado).HasDefaultValue(true);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__645723A671DC67F6");

            entity.Property(e => e.Estado).HasDefaultValue(true);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario__idRol__1332DBDC");
        });

        modelBuilder.Entity<VwOfertasDisponible>(entity =>
        {
            entity.ToView("vwOfertasDisponibles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
