using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

public partial class CargoAdministrativo
{
    [Key]
    [Column("idCargoAdministrativo")]
    public int IdCargoAdministrativo { get; set; }

    [Column("nombreCargo")]
    [StringLength(100)]
    public string NombreCargo { get; set; } = null!;

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdCargoAdministrativoNavigation")]
    public virtual ICollection<Administrativo> Administrativo { get; set; } = new List<Administrativo>();
}
