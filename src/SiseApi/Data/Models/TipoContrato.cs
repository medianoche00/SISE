using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

public partial class TipoContrato
{
    [Key]
    [Column("idTipoContrato")]
    public int IdTipoContrato { get; set; }

    [Column("nombreTipo")]
    [StringLength(100)]
    public string NombreTipo { get; set; } = null!;

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdTipoContratoNavigation")]
    public virtual ICollection<OfertaLaboral> OfertaLaboral { get; set; } = new List<OfertaLaboral>();
}
