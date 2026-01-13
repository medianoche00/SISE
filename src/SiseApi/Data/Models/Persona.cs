using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data.Models;

[Index("IdTipoDocumento", "NumeroDocumento", Name = "UQ_Persona_Documento", IsUnique = true)]
public partial class Persona
{
    [Key]
    [Column("idPersona")]
    public int IdPersona { get; set; }

    [Column("nombres")]
    [StringLength(100)]
    public string Nombres { get; set; } = null!;

    [Column("apellidoPaterno")]
    [StringLength(100)]
    public string ApellidoPaterno { get; set; } = null!;

    [Column("apellidoMaterno")]
    [StringLength(100)]
    public string ApellidoMaterno { get; set; } = null!;

    [Column("numeroDocumento")]
    [StringLength(20)]
    [Unicode(false)]
    public string NumeroDocumento { get; set; } = null!;

    [Column("idTipoDocumento")]
    public int IdTipoDocumento { get; set; }

    [Column("telefono")]
    [StringLength(15)]
    public string? Telefono { get; set; }

    [Column("correoPersonal")]
    [StringLength(150)]
    public string? CorreoPersonal { get; set; }

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdPersonaNavigation")]
    public virtual ICollection<Administrativo> Administrativo { get; set; } = new List<Administrativo>();

    [InverseProperty("IdPersonaNavigation")]
    public virtual ICollection<Egresado> Egresado { get; set; } = new List<Egresado>();

    [ForeignKey("IdTipoDocumento")]
    [InverseProperty("Persona")]
    public virtual TipoDocumento IdTipoDocumentoNavigation { get; set; } = null!;

    [InverseProperty("IdPersonaNavigation")]
    public virtual ICollection<Representante> Representante { get; set; } = new List<Representante>();
}
