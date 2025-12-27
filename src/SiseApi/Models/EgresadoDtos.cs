namespace SiseApi.Models 
{
    public class ActualizarPerfilDto
    {
        public string Telefono { get; set; }
        public string CorreoPersonal { get; set; }
        public string Direccion { get; set; }

        public List<ExperienciaLaboralDto> ExperienciaLaboral { get; set; }
    }

    public class ExperienciaLaboralDto
    {
        public string Empresa { get; set; }
        public string Cargo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool Actualmente { get; set; }
    }
}