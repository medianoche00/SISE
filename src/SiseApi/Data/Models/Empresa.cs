using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

[Index("Ruc", Name = "UQ_Empresa_RUC", IsUnique = true)]
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
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdEmpresaRegistradaNavigation")]
    public virtual ICollection<ExperienciaLaboral> ExperienciaLaboral { get; set; } = new List<ExperienciaLaboral>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<OfertaLaboral> OfertaLaboral { get; set; } = new List<OfertaLaboral>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Representante> Representante { get; set; } = new List<Representante>();
}
