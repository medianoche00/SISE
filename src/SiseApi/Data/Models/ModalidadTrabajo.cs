using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

public partial class ModalidadTrabajo
{
    [Key]
    [Column("idModalidadTrabajo")]
    public int IdModalidadTrabajo { get; set; }

    [Column("nombreModalidad")]
    [StringLength(100)]
    public string NombreModalidad { get; set; } = null!;

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdModalidadTrabajoNavigation")]
    public virtual ICollection<OfertaLaboral> OfertaLaboral { get; set; } = new List<OfertaLaboral>();
}
