using Microsoft.AspNetCore.Identity;
using SiseApi.Models;

namespace SiseApi.Services
{
    public interface ITokenService
    {
        Task<AuthResponse> CreateTokenAsync(IdentityUser<int> user);
    }
}
