using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

public partial class Facultad
{
    [Key]
    [Column("idFacultad")]
    public int IdFacultad { get; set; }

    [Column("nombreFacultad")]
    [StringLength(150)]
    public string NombreFacultad { get; set; } = null!;

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdFacultadNavigation")]
    public virtual ICollection<Escuela> Escuela { get; set; } = new List<Escuela>();
}
