using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Models;

[Table("Carrera")]
public partial class Carrera
{
    [Key]
    [Column("idCarrera")]
    public int IdCarrera { get; set; }

    [Column("idEscuela")]
    public int IdEscuela { get; set; }

    [Column("nombreCarrera")]
    [StringLength(150)]
    public string NombreCarrera { get; set; } = null!;

    [Column("estado")]
    public bool Estado { get; set; }

    [InverseProperty("Carrera")]
    public virtual ICollection<Egresado> Egresados { get; set; } = new List<Egresado>();

    [ForeignKey("IdEscuela")]
    [InverseProperty("Carreras")]
    public virtual Escuela IdEscuelaNavigation { get; set; } = null!;
}
