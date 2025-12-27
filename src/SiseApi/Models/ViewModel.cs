using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SiseApi.Models.ViewModels
{
    public class RegistroEgresadoViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "El apellido paterno es obligatorio")]
        public string ApellidoPaterno { get; set; }

        [Required(ErrorMessage = "El apellido materno es obligatorio")]
        public string ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "El DNI/Documento es obligatorio")]
        [StringLength(20)]
        public string DocumentoIdentidad { get; set; }

        [Phone]
        public string Telefono { get; set; }

        [EmailAddress]
        [Display(Name = "Correo Personal de Contacto")]
        public string CorreoPersonal { get; set; }

        [Required(ErrorMessage = "El código universitario es obligatorio")]
        public string CodigoUniversitario { get; set; }

        [Required(ErrorMessage = "El año de egreso es obligatorio")]
        [Range(1950, 2100, ErrorMessage = "Año inválido")]
        public int AñoEgreso { get; set; }

        [Required(ErrorMessage = "Seleccione su carrera")]
        [Display(Name = "Carrera Profesional")]
        public int IdCarrera { get; set; }

        public IEnumerable<SelectListItem>? ListaCarreras { get; set; }
    }
}