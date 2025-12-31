//using System.ComponentModel.DataAnnotations;
namespace SiseApi.Models
{
    public class ExperienciaLaboralDto
    {
        public int? IdExperiencia { get; set; }

        //[Required(ErrorMessage = "El nombre de la empresa es obligatorio")]
        public string Empresa { get; set; }

        //[Required(ErrorMessage = "El cargo es obligatorio")]
        public string Cargo { get; set; }

        //[Required]
        public DateOnly FechaInicio { get; set; }

        public DateOnly? FechaFin { get; set; }

        public bool Actualmente { get; set; }

        public string? Descripcion { get; set; }
    }
}
