using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

public partial class TipoDocumento
{
    [Key]
    [Column("idTipoDocumento")]
    public int IdTipoDocumento { get; set; }

    [Column("nombreTipo")]
    [StringLength(50)]
    public string NombreTipo { get; set; } = null!;

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdTipoDocumentoNavigation")]
    public virtual ICollection<Persona> Persona { get; set; } = new List<Persona>();
}
