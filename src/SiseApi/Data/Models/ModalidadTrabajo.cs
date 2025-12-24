using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

[Table("ModalidadTrabajo")]
[Index(nameof(NombreModalidad), IsUnique = true)]
public partial class ModalidadTrabajo
{
    [Key][Column("idModalidadTrabajo")] public int IdModalidadTrabajo { get; set; }
    [Column("nombreModalidad")][StringLength(100)] public string NombreModalidad { get; set; } = null!;
    [Column("estado")] public bool Estado { get; set; }
    [InverseProperty("IdModalidadTrabajoNavigation")] public virtual ICollection<OfertaLaboral> OfertaLaborals { get; set; } = new List<OfertaLaboral>();
}