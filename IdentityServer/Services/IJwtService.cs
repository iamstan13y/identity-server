using IdentityServer.Models;

namespace IdentityServer.Services
{
    public interface IJwtService
    {
        Task<string> GenerateToken(Account account);
    }
}
