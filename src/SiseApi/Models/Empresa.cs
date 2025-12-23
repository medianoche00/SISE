using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Models;

[Table("Empresa")]
[Index("Ruc", Name = "UQ__Empresa__C2B74E61241D7335", IsUnique = true)]
public partial class Empresa
{
    [Key]
    [Column("idEmpresa")]
    public int IdEmpresa { get; set; }

    [Column("ruc")]
    [StringLength(11)]
    [Unicode(false)]
    public string Ruc { get; set; } = null!;

    [Column("razonSocial")]
    [StringLength(150)]
    public string RazonSocial { get; set; } = null!;

    [Column("direccion")]
    [StringLength(255)]
    public string? Direccion { get; set; }

    [Column("telefono")]
    [StringLength(15)]
    public string? Telefono { get; set; }

    [Column("correo")]
    [StringLength(150)]
    public string? Correo { get; set; }

    [Column("descripcion")]
    [StringLength(255)]
    public string? Descripcion { get; set; }

    [Column("estado")]
    public bool Estado { get; set; }

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<OfertaLaboral> OfertaLaborals { get; set; } = new List<OfertaLaboral>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Representante> Representantes { get; set; } = new List<Representante>();
}
