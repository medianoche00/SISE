using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

[Keyless]
public partial class VwOfertasDisponible
{
    [Column("idOferta")]
    public int IdOferta { get; set; }

    [Column("titulo")]
    [StringLength(150)]
    public string Titulo { get; set; } = null!;

    [Column("empresa")]
    [StringLength(150)]
    public string Empresa { get; set; } = null!;

    [Column("ubicacion")]
    [StringLength(150)]
    public string? Ubicacion { get; set; }

    [Column("sueldo", TypeName = "decimal(10, 2)")]
    public decimal? Sueldo { get; set; }

    [Column("nombreModalidad")]
    [StringLength(100)]
    public string NombreModalidad { get; set; } = null!;

    [Column("tipoContrato")]
    [StringLength(100)]
    public string TipoContrato { get; set; } = null!;

    [Column("fechaCierre")]
    public DateOnly FechaCierre { get; set; }

    [Column("diasRestantes")]
    public int? DiasRestantes { get; set; }
}
