using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

public partial class TipoFormacion
{
    [Key]
    [Column("idTipoFormacion")]
    public int IdTipoFormacion { get; set; }

    [Column("nombreTipoFormacion")]
    [StringLength(100)]
    public string NombreTipoFormacion { get; set; } = null!;

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdTipoFormacionNavigation")]
    public virtual ICollection<FormacionComplementaria> FormacionComplementaria { get; set; } = new List<FormacionComplementaria>();
}
