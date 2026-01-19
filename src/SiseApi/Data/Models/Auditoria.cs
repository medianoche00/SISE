using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiseApi.Data.Models;

public partial class Auditoria
{
    [Key]
    [Column("idAuditoria")]
    public long IdAuditoria { get; set; }

    [Column("nombreTabla")]
    [StringLength(100)]
    public string NombreTabla { get; set; } = null!;

    [Column("idRegistro")]
    [StringLength(100)]
    public string IdRegistro { get; set; } = null!;

    [Column("tipoAccion")]
    [StringLength(20)]
    public string TipoAccion { get; set; } = null!;

    [Column("idUsuario")]
    public int? IdUsuario { get; set; }

    [Column("usuarioDB")]
    [StringLength(100)]
    public string UsuarioDb { get; set; } = null!;

    [Column("fechaCambio")]
    public DateTime FechaCambio { get; set; }

    [Column("valAntiguos")]
    public string? ValAntiguos { get; set; }

    [Column("valNuevos")]
    public string? ValNuevos { get; set; }

    [Column("docRespaldo")]
    [StringLength(100)]
    public string? DocRespaldo { get; set; }

    [ForeignKey("IdUsuario")]
    public virtual IdentityUser<int>? IdUsuarioNavigation { get; set; }
}
