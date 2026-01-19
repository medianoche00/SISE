using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

public partial class Direccion
{
    [Key]
    [Column("idDireccion")]
    public int IdDireccion { get; set; }

    [Column("idDistrito")]
    public int IdDistrito { get; set; }

    [Column("calle")]
    [StringLength(150)]
    public string Calle { get; set; } = null!;

    [Column("numero")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Numero { get; set; }

    [Column("pisoDepartamento")]
    [StringLength(20)]
    [Unicode(false)]
    public string? PisoDepartamento { get; set; }

    [Column("referencia")]
    [StringLength(200)]
    public string? Referencia { get; set; }

    [Column("fechaRegistro", TypeName = "datetime")]
    public DateTime? FechaRegistro { get; set; }

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdDireccionNavigation")]
    public virtual ICollection<Empresa> Empresa { get; set; } = new List<Empresa>();

    [ForeignKey("IdDistrito")]
    [InverseProperty("Direccion")]
    public virtual Distrito IdDistritoNavigation { get; set; } = null!;

    [InverseProperty("IdDireccionNavigation")]
    public virtual ICollection<OfertaLaboral> OfertaLaboral { get; set; } = new List<OfertaLaboral>();

    [InverseProperty("IdDireccionNavigation")]
    public virtual ICollection<Persona> Persona { get; set; } = new List<Persona>();
}
