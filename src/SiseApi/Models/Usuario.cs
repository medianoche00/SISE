using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Models;

[Table("Usuario")]
[Index("Nomusuario", Name = "UQ__Usuario__9AFF8FC65D5309F3", IsUnique = true)]
public partial class Usuario
{
    [Key]
    [Column("idUsuario")]
    public int IdUsuario { get; set; }

    [Column("nomusuario")]
    [StringLength(50)]
    public string Nomusuario { get; set; } = null!;

    [Column("claveHash")]
    [StringLength(255)]
    public string ClaveHash { get; set; } = null!;

    [Column("idRol")]
    public int IdRol { get; set; }

    [Column("estado")]
    public bool Estado { get; set; }

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Auditoria> Auditoria { get; set; } = new List<Auditoria>();

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Egresado> Egresados { get; set; } = new List<Egresado>();

    [ForeignKey("IdRol")]
    [InverseProperty("Usuarios")]
    public virtual Rol IdRolNavigation { get; set; } = null!;

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Representante> Representantes { get; set; } = new List<Representante>();
}
