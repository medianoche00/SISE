using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Models;

[Table("RolPermiso")]
public partial class RolPermiso
{
    [Key]
    [Column("idRolPermiso")]
    public int IdRolPermiso { get; set; }

    [Column("idRol")]
    public int IdRol { get; set; }

    [Column("idPermiso")]
    public int IdPermiso { get; set; }

    [Column("estado")]
    public bool Estado { get; set; }

    [ForeignKey("IdPermiso")]
    [InverseProperty("RolPermisos")]
    public virtual Permiso IdPermisoNavigation { get; set; } = null!;

    [ForeignKey("IdRol")]
    [InverseProperty("RolPermisos")]
    public virtual Rol IdRolNavigation { get; set; } = null!;
}
