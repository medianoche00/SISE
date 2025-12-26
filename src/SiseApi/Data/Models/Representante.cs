using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

[Table("Representante")]
[Index(nameof(IdUsuario), IsUnique = true)] // Garantiza 1 Usuario = 1 Perfil Representante
public class Representante
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
    public virtual Empresa Empresa { get; set; } = null!;

    [ForeignKey("IdPersona")]
    [InverseProperty("Representantes")]
    public virtual Persona Persona { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    [InverseProperty("Representantes")]
    public virtual ApplicationUser Usuario { get; set; } = null!;
}