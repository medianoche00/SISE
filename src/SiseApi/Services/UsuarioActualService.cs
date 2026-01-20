using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SiseApi.Data;

namespace SiseApi.Services
{
    public class UsuarioActualService : IUsuarioActualService
    {
        private readonly SiseDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioActualService(SiseDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetIdUsuarioLogueado()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null) return null;

            var userIdClaim = user.FindFirst("uid")?.Value
                           ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            return null;
        }

        public async Task<int?> GetIdEgresadoActualAsync()
        {
            var userId = GetIdUsuarioLogueado();
            if (userId == null) return null;

            var egresado = await _context.Egresado
                .AsNoTracking()
                .Select(e => new { e.IdEgresado, e.IdUsuario })
                .FirstOrDefaultAsync(e => e.IdUsuario == userId);

            return egresado?.IdEgresado;
        }

        public async Task<int?> GetIdRepresentanteActualAsync()
        {
            var userId = GetIdUsuarioLogueado();
            if (userId == null) return null;

            var rep = await _context.Representante
                .AsNoTracking()
                .Select(r => new { r.IdRepresentante, r.IdUsuario })
                .FirstOrDefaultAsync(r => r.IdUsuario == userId);

            return rep?.IdRepresentante;
        }

        public async Task<int?> GetIdAdministrativoActualAsync()
        {
            var userId = GetIdUsuarioLogueado();
            if (userId == null) return null;

            var adm = await _context.Administrativo
                .AsNoTracking()
                .Select(r => new { r.IdAdministrativo, r.IdUsuario })
                .FirstOrDefaultAsync(r => r.IdUsuario == userId);

            return adm?.IdAdministrativo;
        }
    }
}
