using Microsoft.EntityFrameworkCore;
using SiseApi.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiseApi.Models;

[Table("Postulacion")]
// Este índice evita que un egresado se postule 2 veces a la misma oferta
[Index(nameof(IdEgresado), nameof(IdOferta), IsUnique = true)]
public class Postulacion
{
    [Key]
    [Column("idPostulacion")]
    public int IdPostulacion { get; set; }

    [Column("idEgresado")]
    public int IdEgresado { get; set; }

    [Column("idOferta")]
    public int IdOferta { get; set; }

    // Puede ser nulo porque al inicio nadie la ha evaluado
    [Column("idRepresentanteEvaluador")]
    public int? IdRepresentanteEvaluador { get; set; }

    [Column("fechaPostulacion", TypeName = "datetime")]
    public DateTime FechaPostulacion { get; set; } = DateTime.Now;

    [Column("fechaEvaluacion", TypeName = "datetime")]
    public DateTime? FechaEvaluacion { get; set; }

    [Column("estado")]
    [StringLength(50)]
    public string Estado { get; set; } = "Pendiente"; // Valor por defecto

    [Column("comentarios")]
    [StringLength(500)]
    public string? Comentarios { get; set; }
    
    [Column("cartaPresentacion")]
    [StringLength(500)]
    public string? CartaPresentacion { get; set; }

    // --- NAVEGACIÓN ---

    [ForeignKey("IdEgresado")]
    [InverseProperty("Postulaciones")]
    public virtual Egresado Egresado { get; set; } = null!;

    [ForeignKey("IdOferta")]
    [InverseProperty("Postulaciones")]
    public virtual OfertaLaboral OfertaLaboral { get; set; } = null!;

    [ForeignKey("IdRepresentanteEvaluador")]
    [InverseProperty("PostulacionesEvaluadas")]
    public virtual Representante? RepresentanteEvaluador { get; set; }
}