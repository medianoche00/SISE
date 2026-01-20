using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

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
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdCarreraNavigation")]
    public virtual ICollection<Egresado> Egresado { get; set; } = new List<Egresado>();

    [ForeignKey("IdEscuela")]
    [InverseProperty("Carrera")]
    public virtual Escuela IdEscuelaNavigation { get; set; } = null!;
}
