using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

public partial class Provincia
{
    [Key]
    [Column("idProvincia")]
    public int IdProvincia { get; set; }

    [Column("idDepartamento")]
    public int IdDepartamento { get; set; }

    [Column("nombreProvincia")]
    [StringLength(100)]
    public string NombreProvincia { get; set; } = null!;

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdProvinciaNavigation")]
    public virtual ICollection<Distrito> Distrito { get; set; } = new List<Distrito>();

    [ForeignKey("IdDepartamento")]
    [InverseProperty("Provincia")]
    public virtual Departamento IdDepartamentoNavigation { get; set; } = null!;
}
