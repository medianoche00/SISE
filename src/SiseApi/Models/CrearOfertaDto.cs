namespace SiseApi.Models
{
    public class CrearOfertaDto
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Requisitos { get; set; }
        public string Ubicacion { get; set; }
        public decimal Sueldo { get; set; }
        public int IdTipoContrato { get; set; }
        public int IdModalidadTrabajo { get; set; }
        //public DateOnly FechaInicio { get; set; }
        public DateOnly FechaCierre { get; set; }
    }
}
