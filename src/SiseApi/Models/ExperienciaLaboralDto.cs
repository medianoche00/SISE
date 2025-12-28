using System.ComponentModel.DataAnnotations;
namespace SiseApi.Models
{
    public class ExperienciaLaboralDto
    {
        public int? IdExperiencia { get; set; } // Null al crear, con valor al editar

        [Required(ErrorMessage = "El nombre de la empresa es obligatorio")]
        public string Empresa { get; set; }

        [Required(ErrorMessage = "El cargo es obligatorio")]
        public string Cargo { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public bool Actualmente { get; set; }

        public string? Descripcion { get; set; }
    }
}
