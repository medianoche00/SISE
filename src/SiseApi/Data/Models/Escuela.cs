using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

public partial class Escuela
{
    [Key]
    [Column("idEscuela")]
    public int IdEscuela { get; set; }

    [Column("idFacultad")]
    public int IdFacultad { get; set; }

    [Column("nombreEscuela")]
    [StringLength(150)]
    public string NombreEscuela { get; set; } = null!;

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdEscuelaNavigation")]
    public virtual ICollection<Carrera> Carrera { get; set; } = new List<Carrera>();

    [ForeignKey("IdFacultad")]
    [InverseProperty("Escuela")]
    public virtual Facultad IdFacultadNavigation { get; set; } = null!;
}
