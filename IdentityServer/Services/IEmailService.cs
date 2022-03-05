using IdentityServer.Models.Data;

namespace IdentityServer.Services
{
    public interface IEmailService
    {
        Task<Result<string>> SendEmailAsync(EmailRequest email);
    }
}
