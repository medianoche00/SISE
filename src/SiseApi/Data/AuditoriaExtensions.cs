using Microsoft.Data.SqlClient;
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
            string docRespaldo,
            int? idUsuario,
            string sql,
            params object[] parameters)
        {
            await context.Database.OpenConnectionAsync();

            try
            {
                if (idUsuario.HasValue)
                {
                    var pIdUsuario = new SqlParameter("@UserId", idUsuario.Value);
                    var pDocRef = new SqlParameter("@DocRef", docRespaldo ?? (object)DBNull.Value);

                    await context.Database.ExecuteSqlRawAsync(
                        "EXEC sp_set_session_context 'AuditUserId', @UserId", pIdUsuario);

                    await context.Database.ExecuteSqlRawAsync(
                        "EXEC sp_set_session_context 'AuditDocRef', @DocRef", pDocRef);
                }

                return await context.Database.ExecuteSqlRawAsync(sql, parameters);
            }
            finally
            {
                await context.Database.CloseConnectionAsync();
            }
        }
    }
}
