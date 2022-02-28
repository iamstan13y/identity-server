namespace IdentityServer.Services
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyHash(string password, string hashedPassword);
    }
}
