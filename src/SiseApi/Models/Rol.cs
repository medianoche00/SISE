using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Models;

[Table("Rol")]
public partial class Rol
{
    [Key]
    [Column("idRol")]
    public int IdRol { get; set; }

    [Column("nombreRol")]
    [StringLength(50)]
    public string NombreRol { get; set; } = null!;

    [Column("descripcion")]
    [StringLength(255)]
    public string? Descripcion { get; set; }

    [Column("estado")]
    public bool Estado { get; set; }

    [InverseProperty("IdRolNavigation")]
    public virtual ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();

    [InverseProperty("IdRolNavigation")]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
