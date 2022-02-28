using System.Security.Cryptography;

namespace IdentityServer.Services
{
    public class PasswordService : IPasswordService
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;

        public static string Hash(string password, int iterations)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            var base64Hash = Convert.ToBase64String(hashBytes);

            return string.Format("$ju!137$V1${0}${1}", iterations, base64Hash);
        }

        public bool VerifyHash(string password, string hashedPassword)
        {
            var splittedHashString = hashedPassword.Replace("$ju!137$V1$", "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            var hashBytes = Convert.FromBase64String(base64Hash);

            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i]) return false;
            }

            return true;
        }

        public string HashPassword(string password)
        {
            return Hash(password, 10000);
        }
    }
}
