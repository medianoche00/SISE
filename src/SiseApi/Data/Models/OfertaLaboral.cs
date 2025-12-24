using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

[Table("OfertaLaboral")]
public partial class OfertaLaboral
{
    [Key]
    [Column("idOferta")]
    public int IdOferta { get; set; }

    [Column("idEmpresa")]
    public int IdEmpresa { get; set; }

    [Column("titulo")]
    [StringLength(150)]
    public string Titulo { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("requisitos")]
    public string? Requisitos { get; set; }

    [Column("ubicacion")]
    [StringLength(150)]
    public string? Ubicacion { get; set; }

    [Column("idTipoContrato")]
    public int IdTipoContrato { get; set; }

    [Column("sueldo", TypeName = "decimal(10, 2)")]
    public decimal? Sueldo { get; set; }

    [Column("idModalidadTrabajo")]
    public int IdModalidadTrabajo { get; set; }

    [Column("fechaPublicacion")]
    public DateOnly FechaPublicacion { get; set; }

    [Column("fechaCierre")]
    public DateOnly FechaCierre { get; set; }

    [Column("idEgresadoGanador")]
    public int? IdEgresadoGanador { get; set; }

    [Column("estado")]
    public bool Estado { get; set; }

    [ForeignKey("IdEgresadoGanador")]
    [InverseProperty("OfertaLaborals")]
    public virtual Egresado? IdEgresadoGanadorNavigation { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("OfertaLaborals")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    [ForeignKey("IdModalidadTrabajo")]
    [InverseProperty("OfertaLaborals")]
    public virtual ModalidadTrabajo IdModalidadTrabajoNavigation { get; set; } = null!;

    [ForeignKey("IdTipoContrato")]
    [InverseProperty("OfertaLaborals")]
    public virtual TipoContrato IdTipoContratoNavigation { get; set; } = null!;
}