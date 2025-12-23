using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Models;

[Table("Persona")]
[Index("Dni", Name = "UQ__Persona__D87608A7915FF7BB", IsUnique = true)]
public partial class Persona
{
    [Key]
    [Column("idPersona")]
    public int IdPersona { get; set; }

    [Column("nombres")]
    [StringLength(100)]
    public string Nombres { get; set; } = null!;

    [Column("apellidoPaterno")]
    [StringLength(100)]
    public string ApellidoPaterno { get; set; } = null!;

    [Column("apellidoMaterno")]
    [StringLength(100)]
    public string ApellidoMaterno { get; set; } = null!;

    [Column("dni")]
    [StringLength(8)]
    [Unicode(false)]
    public string Dni { get; set; } = null!;

    [Column("telefono")]
    [StringLength(15)]
    public string? Telefono { get; set; }

    [Column("correo")]
    [StringLength(150)]
    public string? Correo { get; set; }

    [Column("estado")]
    public bool Estado { get; set; }

    [InverseProperty("IdPersonaNavigation")]
    public virtual ICollection<Egresado> Egresados { get; set; } = new List<Egresado>();

    [InverseProperty("IdPersonaNavigation")]
    public virtual ICollection<Representante> Representantes { get; set; } = new List<Representante>();
}
