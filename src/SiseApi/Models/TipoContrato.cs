using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Models;

[Table("TipoContrato")]
[Index("NombreTipo", Name = "UQ__TipoCont__634171E76788CD20", IsUnique = true)]
public partial class TipoContrato
{
    [Key]
    [Column("idTipoContrato")]
    public int IdTipoContrato { get; set; }

    [Column("nombreTipo")]
    [StringLength(100)]
    public string NombreTipo { get; set; } = null!;

    [Column("estado")]
    public bool Estado { get; set; }

    [InverseProperty("IdTipoContratoNavigation")]
    public virtual ICollection<OfertaLaboral> OfertaLaborals { get; set; } = new List<OfertaLaboral>();
}
