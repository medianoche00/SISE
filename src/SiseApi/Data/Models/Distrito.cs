using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

public partial class Distrito
{
    [Key]
    [Column("idDistrito")]
    public int IdDistrito { get; set; }

    [Column("idProvincia")]
    public int IdProvincia { get; set; }

    [Column("nombreDistrito")]
    [StringLength(100)]
    public string NombreDistrito { get; set; } = null!;

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdDistritoNavigation")]
    public virtual ICollection<Direccion> Direccion { get; set; } = new List<Direccion>();

    [ForeignKey("IdProvincia")]
    [InverseProperty("Distrito")]
    public virtual Provincia IdProvinciaNavigation { get; set; } = null!;
}
