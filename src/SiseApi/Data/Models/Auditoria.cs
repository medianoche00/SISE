using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

[Table("Auditoria")]
public partial class Auditoria
{
    [Key]
    [Column("idAuditoria")]
    public int IdAuditoria { get; set; }

    [Column("idUsuario")]
    public int? IdUsuario { get; set; }

    [Column("tablaAfectada")]
    [StringLength(100)]
    public string TablaAfectada { get; set; } = null!;

    [Column("columnaAfectada")]
    [StringLength(100)]
    public string? ColumnaAfectada { get; set; }

    [Column("accion")]
    [StringLength(20)]
    public string Accion { get; set; } = null!;

    [Column("valorAnterior")]
    public string? ValorAnterior { get; set; }

    [Column("valorNuevo")]
    public string? ValorNuevo { get; set; }

    // DateTime mapea correctamente a datetime2(7)
    [Column("fechaHora")]
    public DateTime FechaHora { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("Auditorias")]
    public virtual ApplicationUser? Usuario { get; set; }
}