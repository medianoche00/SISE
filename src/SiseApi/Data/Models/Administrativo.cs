using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiseApi.Data.Models;

public partial class Administrativo
{
    [Key]
    [Column("idAdministrativo")]
    public int IdAdministrativo { get; set; }

    [Column("idCargoAdministrativo")]
    public int IdCargoAdministrativo { get; set; }

    [Column("idPersona")]
    public int IdPersona { get; set; }

    [Column("idUsuario")]
    public int IdUsuario { get; set; }

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [ForeignKey("IdCargoAdministrativo")]
    [InverseProperty("Administrativo")]
    public virtual CargoAdministrativo IdCargoAdministrativoNavigation { get; set; } = null!;

    [ForeignKey("IdPersona")]
    [InverseProperty("Administrativo")]
    public virtual Persona IdPersonaNavigation { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    //[InverseProperty("Administrativo")]
    public virtual IdentityUser<int>? IdUsuarioNavigation { get; set; } = null!;
}
