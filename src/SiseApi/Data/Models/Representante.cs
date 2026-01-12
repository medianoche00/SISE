using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiseApi.Data.Models;

public partial class Representante
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
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [ForeignKey("IdEmpresa")]
    [InverseProperty("Representante")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    [ForeignKey("IdPersona")]
    [InverseProperty("Representante")]
    public virtual Persona IdPersonaNavigation { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    //[InverseProperty("Representante")]
    public virtual IdentityUser<int>? IdUsuarioNavigation { get; set; } = null!;

    [InverseProperty("IdRepresentanteEvaluadorNavigation")]
    public virtual ICollection<Postulacion> Postulacion { get; set; } = new List<Postulacion>();
}
