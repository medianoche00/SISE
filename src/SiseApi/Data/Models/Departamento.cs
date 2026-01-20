using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

public partial class Departamento
{
    [Key]
    [Column("idDepartamento")]
    public int IdDepartamento { get; set; }

    [Column("nombreDepartamento")]
    [StringLength(100)]
    public string NombreDepartamento { get; set; } = null!;

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdDepartamentoNavigation")]
    public virtual ICollection<Provincia> Provincia { get; set; } = new List<Provincia>();
}
