using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Models;

[Table("Facultad")]
public partial class Facultad
{
    [Key]
    [Column("idFacultad")]
    public int IdFacultad { get; set; }

    [Column("nombreFacultad")]
    [StringLength(150)]
    public string NombreFacultad { get; set; } = null!;

    [Column("estado")]
    public bool Estado { get; set; }

    [InverseProperty("IdFacultadNavigation")]
    public virtual ICollection<Escuela> Escuelas { get; set; } = new List<Escuela>();
}
