using System.ComponentModel.DataAnnotations;

namespace SiseApi.Models
{
    public class RepresentanteDto
    {
        public int IdRepresentante { get; set; }
        public int IdEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public int IdPersona { get; set; }
        public int IdUsuario { get; set; }
        public string Cargo { get; set; }
        public string Estado { get; set; }
    }

    public class RepresentanteCrearDto
    {
        // --- Datos para Identity (Creación de usuario) ---
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Password { get; set; }

        // --- Datos de Negocio (Tabla Representante) ---
        [Required(ErrorMessage = "El ID de la empresa es obligatorio.")]
        public int IdEmpresa { get; set; }

        [Required(ErrorMessage = "El ID de la persona es obligatorio.")]
        public int IdPersona { get; set; }

        [StringLength(100, ErrorMessage = "El cargo no puede exceder los 100 caracteres.")]
        public string Cargo { get; set; }

        // --- Auditoría ---
        [Required(ErrorMessage = "El documento de respaldo es obligatorio.")]
        public string DocumentoRespaldo { get; set; }
    }

    public class RepresentanteActualizarDto
    {
        [Required(ErrorMessage = "El ID del representante es obligatorio.")]
        public int IdRepresentante { get; set; }

        [Required(ErrorMessage = "El ID de la empresa es obligatorio.")]
        public int IdEmpresa { get; set; }

        [StringLength(100, ErrorMessage = "El cargo no puede exceder los 100 caracteres.")]
        public string Cargo { get; set; }

        // --- Auditoría ---
        [Required(ErrorMessage = "El documento de respaldo es obligatorio.")]
        public string DocumentoRespaldo { get; set; }
    }
}
