using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

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
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [ForeignKey("IdEgresadoGanador")]
    [InverseProperty("OfertaLaboral")]
    public virtual Egresado? IdEgresadoGanadorNavigation { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("OfertaLaboral")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    [ForeignKey("IdModalidadTrabajo")]
    [InverseProperty("OfertaLaboral")]
    public virtual ModalidadTrabajo IdModalidadTrabajoNavigation { get; set; } = null!;

    [ForeignKey("IdTipoContrato")]
    [InverseProperty("OfertaLaboral")]
    public virtual TipoContrato IdTipoContratoNavigation { get; set; } = null!;

    [InverseProperty("IdOfertaNavigation")]
    public virtual ICollection<Postulacion> Postulacion { get; set; } = new List<Postulacion>();
}
