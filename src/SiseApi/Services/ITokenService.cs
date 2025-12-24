using SiseApi.Data.Models;
using SiseApi.Models;

namespace SiseApi.Services
{
    public interface ITokenService
    {
        Task<AuthResponse> CreateTokenAsync(ApplicationUser user);
    }
}
