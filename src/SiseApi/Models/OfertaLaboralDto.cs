namespace SiseApi.Models
{
    public class OfertaLaboralDto
    {
        public int IdOferta { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Requisitos { get; set; }
        public string Ubicacion { get; set; }
        public decimal Sueldo { get; set; }
        public DateOnly FechaPublicacion { get; set; }
        public DateOnly FechaCierre { get; set; }
        public string TipoContrato { get; set; }
        public string Modalidad { get; set; }

        // Datos de la empresa
        //public string EmpresaNombre { get; set; }
        public string EmpresaRuc { get; set; }
        public string EmpresaRazonSocial { get; set; }
        public string EmpresaTelefono { get; set; }
        public string EmpresaCorreo { get; set; }
    }
}
