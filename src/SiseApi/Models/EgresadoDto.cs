namespace SiseApi.Models
{
    public class EgresadoDto
    {
        public int IdEgresado { get; set; }
        public int IdCarrera { get; set; }
        public string NombreCarrera { get; set; } = null!;
        public string CodigoUniversitario { get; set; } = null!;
        public int AnioEgreso { get; set; }
        public string Estado { get; set; } = null!;
    }

    public class EgresadoCrearDto
    {
        public int IdPersona { get; set; }
        public int IdCarrera { get; set; }
        public int AnioEgreso { get; set; }
        public string CodigoUniversitario { get; set; } = null!;
        public string DocumentoRespaldo { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class EgresadoActualizarDto
    {
        public int IdEgresado { get; set; }
        public int IdCarrera { get; set; }
        public int AnioEgreso { get; set; }
        public string CodigoUniversitario { get; set; } = null!;
        public string DocumentoRespaldo { get; set; } = null!;
        public string Estado { get; set; } = null!;
    }
}
