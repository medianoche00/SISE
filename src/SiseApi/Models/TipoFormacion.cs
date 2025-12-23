using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Models;

[Table("TipoFormacion")]
public partial class TipoFormacion
{
    [Key]
    [Column("idTipoFormacion")]
    public int IdTipoFormacion { get; set; }

    [Column("nombreTipoFormacion")]
    [StringLength(100)]
    public string NombreTipoFormacion { get; set; } = null!;

    [Column("estado")]
    public bool Estado { get; set; }

    [InverseProperty("IdTipoFormacionNavigation")]
    public virtual ICollection<FormacionComplementaria> FormacionComplementaria { get; set; } = new List<FormacionComplementaria>();
}
