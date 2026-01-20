namespace SiseApi.Models
{

    public class UbicacionFlatDto //esto se recibe de la db
    {
        public int IdDepartamento { get; set; }
        public string NombreDepartamento { get; set; }
        public int IdProvincia { get; set; }
        public string NombreProvincia { get; set; }
        public int IdDistrito { get; set; }
        public string NombreDistrito { get; set; }
    }
    public class DepartamentoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<ProvinciaDto> Provincias { get; set; } = new List<ProvinciaDto>();
    }

    public class ProvinciaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<DistritoDto> Distritos { get; set; } = new List<DistritoDto>();
    }

    public class DistritoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
