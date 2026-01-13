
namespace SiseApi.Models
{
    public class PersonaDto
    {
        
        public int IdPersona { get; set; }
        public string Nombres { get; set; } = null!;
        public string ApellidoPaterno { get; set; } = null!;
        public string ApellidoMaterno { get; set; } = null!;
        public string NumeroDocumento { get; set; } = null!;
        public int IdTipoDocumento { get; set; }
        public string NombreTipoDocumento { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? CorreoPersonal { get; set; }
        public string Estado { get; set; } = null!;
    }
}
