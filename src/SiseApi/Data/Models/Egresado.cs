using Microsoft.EntityFrameworkCore;
using SiseApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiseApi.Data.Models;

[Table("Egresado")]
[Index(nameof(CodigoUniversitario), IsUnique = true)]
[Index(nameof(IdUsuario), IsUnique = true)] // Garantiza 1 Usuario = 1 Perfil Egresado
public class Egresado
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
    public bool Estado { get; set; }

    // Navegaciones
    [ForeignKey("IdPersona")]
    [InverseProperty("Egresados")]
    public virtual Persona Persona { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    [InverseProperty("Egresados")]
    public virtual ApplicationUser Usuario { get; set; } = null!;

    [ForeignKey("IdCarrera")]
    [InverseProperty("Egresados")]
    public virtual Carrera Carrera { get; set; } = null!;

    // Colecciones hijas
    [InverseProperty("IdEgresadoNavigation")]
    public virtual ICollection<ExperienciaLaboral> ExperienciaLaborals { get; set; } = new List<ExperienciaLaboral>();

    [InverseProperty("IdEgresadoNavigation")]
    public virtual ICollection<FormacionComplementaria> FormacionComplementaria { get; set; } = new List<FormacionComplementaria>();

    [InverseProperty("IdEgresadoGanadorNavigation")]
    public virtual ICollection<OfertaLaboral> OfertaLaborals { get; set; } = new List<OfertaLaboral>();

    [InverseProperty("Egresado")]
    public virtual ICollection<Postulacion> Postulaciones { get; set; } = new List<Postulacion>();
}