using System.ComponentModel.DataAnnotations;

namespace SiseApi.Models
{
    // DTO para Listar / Obtener (Salida)
    public class PersonaDto
    {
        public int IdPersona { get; set; }
        public string Nombres { get; set; } = null!;
        public string ApellidoPaterno { get; set; } = null!;
        public string ApellidoMaterno { get; set; } = null!;
        public string NumeroDocumento { get; set; } = null!;
        public int IdTipoDocumento { get; set; }
        public string NombreTipoDocumento { get; set; } = null!; // Campo de lectura (JOIN)
        public string? Telefono { get; set; }
        public string? CorreoPersonal { get; set; }

        // Dirección (Lectura)
        public int IdDireccion { get; set; }
        public int IdDistrito { get; set; }
        //public string NombreDistrito { get; set; } // Opcional: útil para listar
        public string Calle { get; set; } = null!;
        public string? Numero { get; set; }
        public string? PisoDepartamento { get; set; }
        public string? Referencia { get; set; }

        public string Estado { get; set; } = null!;
    }

    // DTO para Crear (Entrada)
    public class PersonaCrearDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "El apellido paterno es obligatorio.")]
        [StringLength(100)]
        public string ApellidoPaterno { get; set; } = null!;

        [Required(ErrorMessage = "El apellido materno es obligatorio.")]
        [StringLength(100)]
        public string ApellidoMaterno { get; set; } = null!;

        [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
        public int IdTipoDocumento { get; set; }

        [Required(ErrorMessage = "El número de documento es obligatorio.")]
        [StringLength(20)]
        public string NumeroDocumento { get; set; } = null!;

        [StringLength(20)]
        public string? Telefono { get; set; }

        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        [StringLength(100)]
        public string? CorreoPersonal { get; set; }

        // --- Datos de Dirección ---
        [Required(ErrorMessage = "El distrito es obligatorio.")]
        public int IdDistrito { get; set; }

        [Required(ErrorMessage = "La calle es obligatoria.")]
        [StringLength(200)]
        public string Calle { get; set; } = null!;

        [StringLength(20)]
        public string? Numero { get; set; }

        [StringLength(20)]
        public string? PisoDepartamento { get; set; }

        [StringLength(200)]
        public string? Referencia { get; set; }

        // --- Auditoría ---
        [Required(ErrorMessage = "El documento de respaldo es obligatorio.")]
        public string DocumentoRespaldo { get; set; } = null!;
    }

    // DTO para Actualizar (Entrada)
    public class PersonaActualizarDto
    {
        [Required(ErrorMessage = "El ID de la persona es obligatorio.")]
        public int IdPersona { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "El apellido paterno es obligatorio.")]
        [StringLength(100)]
        public string ApellidoPaterno { get; set; } = null!;

        [Required(ErrorMessage = "El apellido materno es obligatorio.")]
        [StringLength(100)]
        public string ApellidoMaterno { get; set; } = null!;

        [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
        public int IdTipoDocumento { get; set; }

        [Required(ErrorMessage = "El número de documento es obligatorio.")]
        [StringLength(20)]
        public string NumeroDocumento { get; set; } = null!;

        [StringLength(20)]
        public string? Telefono { get; set; }

        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        [StringLength(100)]
        public string? CorreoPersonal { get; set; }

        // --- Datos de Dirección ---
        [Required(ErrorMessage = "El distrito es obligatorio.")]
        public int IdDistrito { get; set; }

        [Required(ErrorMessage = "La calle es obligatoria.")]
        [StringLength(200)]
        public string Calle { get; set; } = null!;

        [StringLength(20)]
        public string? Numero { get; set; }

        [StringLength(20)]
        public string? PisoDepartamento { get; set; }

        [StringLength(200)]
        public string? Referencia { get; set; }

        // --- Auditoría ---
        [Required(ErrorMessage = "El documento de respaldo es obligatorio.")]
        public string DocumentoRespaldo { get; set; } = null!;
    }
}