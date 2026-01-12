using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiseApi.Data.Models;

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

    [Column("fechaHora")]
    public DateTime FechaHora { get; set; }
}
