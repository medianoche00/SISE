using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

public partial class Postulacion
{
    [Key]
    [Column("idPostulacion")]
    public int IdPostulacion { get; set; }

    [Column("idEgresado")]
    public int IdEgresado { get; set; }

    [Column("idOferta")]
    public int IdOferta { get; set; }

    [Column("idRepresentanteEvaluador")]
    public int? IdRepresentanteEvaluador { get; set; }

    [Column("fechaPostulacion", TypeName = "datetime")]
    public DateTime FechaPostulacion { get; set; }

    [Column("fechaEvaluacion", TypeName = "datetime")]
    public DateTime? FechaEvaluacion { get; set; }

    [Column("comentarios")]
    [StringLength(500)]
    public string? Comentarios { get; set; }

    [Column("cartaPresentacion")]
    [StringLength(500)]
    public string? CartaPresentacion { get; set; }

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [ForeignKey("IdEgresado")]
    [InverseProperty("Postulacion")]
    public virtual Egresado IdEgresadoNavigation { get; set; } = null!;

    [ForeignKey("IdOferta")]
    [InverseProperty("Postulacion")]
    public virtual OfertaLaboral IdOfertaNavigation { get; set; } = null!;

    [ForeignKey("IdRepresentanteEvaluador")]
    [InverseProperty("Postulacion")]
    public virtual Representante? IdRepresentanteEvaluadorNavigation { get; set; }
}
