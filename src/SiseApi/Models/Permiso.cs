using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Models;

[Table("Permiso")]
public partial class Permiso
{
    [Key]
    [Column("idPermiso")]
    public int IdPermiso { get; set; }

    [Column("nombrePermiso")]
    [StringLength(100)]
    public string NombrePermiso { get; set; } = null!;

    [Column("descripcion")]
    [StringLength(255)]
    public string? Descripcion { get; set; }

    [Column("estado")]
    public bool Estado { get; set; }

    [InverseProperty("IdPermisoNavigation")]
    public virtual ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();
}
