using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

[Index("CodigoUniversitario", Name = "UQ_Egresado_Codigo", IsUnique = true)]
public partial class Egresado
{
    [Key]
    [Column("idEgresado")]
    public int IdEgresado { get; set; }

    [Column("idPersona")]
    public int IdPersona { get; set; }

    [Column("idUsuario")]
    public int IdUsuario { get; set; }

    [Column("idCarrera")]
    public int IdCarrera { get; set; }

    [Column("codigoUniversitario")]
    [StringLength(20)]
    public string CodigoUniversitario { get; set; } = null!;

    [Column("añoEgreso")]
    public int AñoEgreso { get; set; }

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdEgresadoNavigation")]
    public virtual ICollection<ExperienciaLaboral> ExperienciaLaboral { get; set; } = new List<ExperienciaLaboral>();

    [InverseProperty("IdEgresadoNavigation")]
    public virtual ICollection<FormacionComplementaria> FormacionComplementaria { get; set; } = new List<FormacionComplementaria>();

    [ForeignKey("IdCarrera")]
    [InverseProperty("Egresado")]
    public virtual Carrera IdCarreraNavigation { get; set; } = null!;

    [ForeignKey("IdPersona")]
    [InverseProperty("Egresado")]
    public virtual Persona IdPersonaNavigation { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    //[InverseProperty("Egresado")]
    public virtual IdentityUser<int>? IdUsuarioNavigation { get; set; } = null!;

    [InverseProperty("IdEgresadoGanadorNavigation")]
    public virtual ICollection<OfertaLaboral> OfertaLaboral { get; set; } = new List<OfertaLaboral>();

    [InverseProperty("IdEgresadoNavigation")]
    public virtual ICollection<Postulacion> Postulacion { get; set; } = new List<Postulacion>();
}
