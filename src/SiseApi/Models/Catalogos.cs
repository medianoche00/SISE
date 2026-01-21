using System.Globalization;

namespace SiseApi.Models
{
    public class RolDto
    {
        public int IdRol { get; set; }
        public string NombreRol { get; set; } = null!;
    }

    public class CarreraDto
    {
        public int IdCarrera { get; set; }
        public string NombreCarrera { get; set; } = null!;
        public string Estado { get; set; } = null!;
    }

    public class CargoAdministrativoDto
    {
        public int IdCargoAdministrativo { get; set; }
        public string NombreCargo{ get; set; } = null!;
        public string Estado { get; set; } = null!;
    }
    public class EmpresaDto
    {
        public int IdEmpresa { get; set; }
        public int IdDireccion { get; set; }
        public int IdDistrito { get; set; }
        public string? NombreDistrito { get; set; }
        public string? Calle { get; set; }
        public string? Numero { get; set; }
        public string? Ruc { get; set; }
        public string? RazonSocial { get; set; }
        public string Telefono { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string? Estado { get; set; }
    }

    public class EmpresaMinDto
    {
        public int IdEmpresa { get; set; }
        public string? RazonSocial { get; set; }
        public string? Estado { get; set; }
    }
}
