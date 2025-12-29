

namespace SiseApi.Models
{
    public class MisPostulacionesDto
    {
        public int IdPostulacion { get; set; }
        public int? IdRepresentanteEvaluador { get; set; }
        public DateTime FechaPostulacion { get; set; }
        public DateTime? FechaEvaluacion { get; set; }
        public string Estado { get; set; }
        public string Comentarios { get; set; }
        public string CartaPresentacion { get; set; }

        public OfertaLaboralDto Oferta { get; set; }
    }
}
