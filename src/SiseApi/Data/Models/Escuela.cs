using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

[Table("Escuela")]
public partial class Escuela
{
    [Key][Column("idEscuela")] public int IdEscuela { get; set; }
    [Column("idFacultad")] public int IdFacultad { get; set; }
    [Column("nombreEscuela")][StringLength(150)] public string NombreEscuela { get; set; } = null!;
    [Column("estado")] public bool Estado { get; set; }
    [InverseProperty("IdEscuelaNavigation")] public virtual ICollection<Carrera> Carreras { get; set; } = new List<Carrera>();
    [ForeignKey("IdFacultad")][InverseProperty("Escuelas")] public virtual Facultad IdFacultadNavigation { get; set; } = null!;
}