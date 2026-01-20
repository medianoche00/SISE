using Microsoft.EntityFrameworkCore;

namespace SiseApi.Data
{
    public static class AuditoriaExtensions
    {
        // Método de extensión para el DbContext
        // que ejecuta un procedimiento almacenado previamente configurando el contexto de auditoría
        // para guardar el ID del usuario que realiza la operación.
        public static async Task<int> EjecutarSpConAuditoriaAsync(
            this SiseDbContext context,
            string docRespaldo, // numero del documento que respalda la operacion
            int? idUsuario,
            string sql,
            params object[] parameters)
        {
            await context.Database.OpenConnectionAsync();

            try
            {
                if (idUsuario.HasValue)
                {
                    await context.Database.ExecuteSqlRawAsync(
                        $"EXEC sp_set_session_context 'AuditUserId', {idUsuario.Value}");
                    await context.Database.ExecuteSqlRawAsync(
                        $"EXEC sp_set_session_context 'AuditDocRef', {docRespaldo}");
                }

                return await context.Database.ExecuteSqlRawAsync(sql, parameters);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await context.Database.CloseConnectionAsync();
            }
        }
    }
}
