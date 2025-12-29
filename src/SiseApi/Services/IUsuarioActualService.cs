namespace SiseApi.Services
{
    public interface IUsuarioActualService
    {
        Task<int?> GetIdEgresadoActualAsync();
        Task<int?> GetIdRepresentanteActualAsync();
        Task<int?> GetIdAdministrativoActualAsync();
        int? GetIdUsuarioLogueado();
    }
}
