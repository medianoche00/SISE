using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiseApi.Data.Models
{
    [Table("Administrativo")]
    [Index(nameof(IdUsuario), IsUnique = true)]
    public class Administrativo
    {
        [Key]
        [Column("idAdministrativo")] 
        public int IdAdministrativo { get; set; }
        
        [Column("idCargoAdministrativo")]
        [StringLength(50)] 
        public int IdCargoAdministrativo { get; set; }

        [Column("idPersona")]
        public int IdPersona { get; set; }

        [Column("idUsuario")]
        public int IdUsuario { get; set; }

        [Column("estado")]
        public bool Estado { get; set; }

        [ForeignKey("IdCargoAdministrativo")]
        [InverseProperty("Administrativos")]
        public CargoAdministrativo CargoAdministrativo { get; set; } = null!;

        [ForeignKey("IdPersona")]
        [InverseProperty("Administrativos")]
        public Persona Persona { get; set; } = null!;

        [ForeignKey("IdUsuario")]
        [InverseProperty("Administrativos")]
        public ApplicationUser Usuario { get; set; } = null!;

    }
}
