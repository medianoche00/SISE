using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Models;

[Table("Egresado")]
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
    [StringLength(10)]
    public string CodigoUniversitario { get; set; } = null!;

    [Column("añoEgreso")]
    public int AñoEgreso { get; set; }

    [Column("estado")]
    public bool Estado { get; set; }

    [InverseProperty("IdEgresadoNavigation")]
    public virtual ICollection<ExperienciaLaboral> ExperienciaLaborals { get; set; } = new List<ExperienciaLaboral>();

    [InverseProperty("IdEgresadoNavigation")]
    public virtual ICollection<FormacionComplementaria> FormacionComplementaria { get; set; } = new List<FormacionComplementaria>();

    [ForeignKey("IdCarrera")]
    [InverseProperty("Egresados")]
    public virtual Carrera Carrera { get; set; } = null!;

    [ForeignKey("IdPersona")]
    [InverseProperty("Egresados")]
    public virtual Persona Persona { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    [InverseProperty("Egresados")]
    public virtual ApplicationUser? Usuario { get; set; } = null!;

    [InverseProperty("IdEgresadoGanadorNavigation")]
    public virtual ICollection<OfertaLaboral> OfertaLaborals { get; set; } = new List<OfertaLaboral>();
}
