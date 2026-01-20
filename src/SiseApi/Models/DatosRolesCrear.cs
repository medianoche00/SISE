namespace SiseApi.Models
{
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

    public class AdministrativoCrearDto
    {
        public int IdPersona { get; set; }
        public int IdDepartamento { get; set; }
        public int IdCargoAdministrativo { get; set; }
        public string DocumentoRespaldo { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class RepresentanteCrearDto
    {
        public int IdPersona { get; set; }
        public int IdEmpresa { get; set; }
        public string Cargo { get; set; } = null!;
        public string DocumentoRespaldo { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
