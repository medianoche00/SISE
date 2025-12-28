using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiseApi.Data.Models
{
    [Table("CargoAdministrativo")]
    public class CargoAdministrativo
    {
        [Key]
        [Column("idCargoAdministrativo")] 
        public int IdCargoAdministrativo { get; set; }

        [Column("nombreCargo")][StringLength(100)]
        public string NombreCargo { get; set; } = null!;

        [InverseProperty("CargoAdministrativo")] 
        public virtual ICollection<Administrativo> Administrativos { get; set; } = new List<Administrativo>();
    }
}
