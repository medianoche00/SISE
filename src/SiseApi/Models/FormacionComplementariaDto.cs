namespace SiseApi.Models
{
    public class FormacionComplementariaDto
    {
        public int? IdFormacion { get; set; }
        public int? IdTipoFormacion { get; set; }
        public string TipoFormacion { get; set; }
        public string NombreDelCurso { get; set; }
        public string? Institucion { get; set; }
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }
        public bool Estado { get; set; }
    }
}
