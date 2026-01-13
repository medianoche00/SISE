namespace SiseApi.Models
{
    public class PerfilEgresadoDto
    {
        // Datos Personales (Solo lectura)
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string NumeroDocumento { get; set; }

        // Datos de Contacto (Editables)
        public string Telefono { get; set; }
        public string CorreoPersonal { get; set; }

        // Datos Académicos (Solo lectura)
        public int IdCarrera { get; set; }
        public string CodigoUniversitario { get; set; }
        public int AñoEgreso { get; set; }
        public string Carrera { get; set; }
    }
}
