using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

public partial class FormacionComplementaria
{
    [Key]
    [Column("idFormacion")]
    public int IdFormacion { get; set; }

    [Column("idEgresado")]
    public int IdEgresado { get; set; }

    [Column("idTipoFormacion")]
    public int IdTipoFormacion { get; set; }

    [Column("nombreDelCurso")]
    [StringLength(150)]
    public string NombreDelCurso { get; set; } = null!;

    [Column("institucion")]
    [StringLength(150)]
    public string? Institucion { get; set; }

    [Column("fechaInicio")]
    public DateOnly? FechaInicio { get; set; }

    [Column("fechaFin")]
    public DateOnly? FechaFin { get; set; }

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [ForeignKey("IdEgresado")]
    [InverseProperty("FormacionComplementaria")]
    public virtual Egresado IdEgresadoNavigation { get; set; } = null!;

    [ForeignKey("IdTipoFormacion")]
    [InverseProperty("FormacionComplementaria")]
    public virtual TipoFormacion IdTipoFormacionNavigation { get; set; } = null!;
}
