namespace SiseApi.Models
{
    public class ExpedienteDto
    {

        //{ rol: "Egresado", contexto: "Ing. Sistemas (2022)", usuario: "egre14", idRol: 50, idUsuario: 8, estadoRol: "Trabajando", estadoUsuario: "Activo" },
        public string Rol { get; set; } = null!;
        public string Contexto { get; set; } = null!;
        public string Usuario { get; set; } = null!;
        public int IdRol { get; set; }
        public int IdUsuario { get; set; }
        public string EstadoRol { get; set; } = null!;//el estado de adminstrativo, egresado o representante
        //public string EstadoUsuario { get; set; } = null!;

    }
}
