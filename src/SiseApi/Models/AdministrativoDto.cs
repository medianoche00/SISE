    using System.ComponentModel.DataAnnotations;

namespace SiseApi.Models
{
    public class AdministrativoDto
    {
        public int IdAdministrativo { get; set; }
        public int IdCargoAdministrativo { get; set; }
        public string NombreCargo { get; set; } // Viene del JOIN en el SP
        public int IdPersona { get; set; }
        public int IdUsuario { get; set; }
        public string Estado { get; set; }
    }


public class AdministrativoCrearDto
    {
        // --- Datos para Identity (Usuario) ---
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Password { get; set; }

        // --- Datos de Negocio (Tabla Administrativo) ---
        [Required(ErrorMessage = "El ID del cargo administrativo es obligatorio.")]
        public int IdCargoAdministrativo { get; set; }

        [Required(ErrorMessage = "El ID de la persona es obligatorio.")]
        public int IdPersona { get; set; }

        // --- Auditoría ---
        [Required(ErrorMessage = "El documento de respaldo es obligatorio.")]
        public string DocumentoRespaldo { get; set; }
    }
    public class AdministrativoActualizarDto
    {
        [Required(ErrorMessage = "El ID del administrativo es obligatorio.")]
        public int IdAdministrativo { get; set; }

        [Required(ErrorMessage = "El ID del cargo administrativo es obligatorio.")]
        public int IdCargoAdministrativo { get; set; }

        // --- Auditoría ---
        [Required(ErrorMessage = "El documento de respaldo es obligatorio.")]
        public string DocumentoRespaldo { get; set; }
    }
}
