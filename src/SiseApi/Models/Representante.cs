using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Models;

[Table("Representante")]
public partial class Representante
{
    [Key]
    [Column("idRepresentante")]
    public int IdRepresentante { get; set; }

    [Column("idEmpresa")]
    public int IdEmpresa { get; set; }

    [Column("idPersona")]
    public int IdPersona { get; set; }

    [Column("idUsuario")]
    public int IdUsuario { get; set; }

    [Column("cargo")]
    [StringLength(100)]
    public string? Cargo { get; set; }

    [Column("estado")]
    public bool Estado { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("Representantes")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    [ForeignKey("IdPersona")]
    [InverseProperty("Representantes")]
    public virtual Persona IdPersonaNavigation { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    [InverseProperty("Representantes")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
