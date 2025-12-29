namespace SiseApi.Models
{
    public class PostulanteDetalleDto
    {
        public int IdPostulacion { get; set; }
        public string CartaPresentacion { get; set; }
        public DateTime FechaPostulacion { get; set; }

        public string NombreCompleto { get; set; }
        public string Dni { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Carrera { get; set; }

        public List<ExperienciaLaboralDto> ExperienciaLaboral { get; set; }
        public List<FormacionComplementariaDto> FormacionComplementaria { get; set; }
    }
}
