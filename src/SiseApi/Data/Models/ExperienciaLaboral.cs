using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

[Table("ExperienciaLaboral")]
public partial class ExperienciaLaboral
{
    [Key]
    [Column("idExperiencia")]
    public int IdExperiencia { get; set; }

    [Column("idEgresado")]
    public int IdEgresado { get; set; }

    [Column("empresa")]
    [StringLength(150)]
    public string Empresa { get; set; } = null!;

    // Opcional: si decidiste agregar idEmpresaRegistrada en el SQL
    [Column("idEmpresaRegistrada")]
    public int? IdEmpresaRegistrada { get; set; }

    [Column("cargo")]
    [StringLength(150)]
    public string Cargo { get; set; } = null!;

    [Column("fechaInicio")]
    public DateOnly FechaInicio { get; set; }

    [Column("fechaFin")]
    public DateOnly? FechaFin { get; set; }

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("estado")]
    public bool Estado { get; set; }

    [ForeignKey("IdEgresado")]
    [InverseProperty("ExperienciaLaborals")]
    public virtual Egresado IdEgresadoNavigation { get; set; } = null!;

    [ForeignKey("IdEmpresaRegistrada")]
    public virtual Empresa? EmpresaRegistrada { get; set; }
}