using System.Security.Cryptography;
using System.Text;

namespace GameStoreMini.Utils
{
    // Simple PBKDF2 password hashing helper.
    public static class PasswordHelper
    {
        // Hash a password: returns base64(salt + hash)
        public static string HashPassword(string password)
        {
            // recommended sizes
            var salt = RandomNumberGenerator.GetBytes(16);
            var iter = 100_000;
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iter, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(32);

            var combined = new byte[salt.Length + hash.Length];
            Buffer.BlockCopy(salt, 0, combined, 0, salt.Length);
            Buffer.BlockCopy(hash, 0, combined, salt.Length, hash.Length);
            return Convert.ToBase64String(combined);
        }

        // Verify a password against stored base64(salt+hash)
        public static bool VerifyPassword(string storedBase64, string password)
        {
            var combined = Convert.FromBase64String(storedBase64);
            var salt = combined[..16];
            var hash = combined[16..];

            var iter = 100_000;
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iter, HashAlgorithmName.SHA256);
            var computed = pbkdf2.GetBytes(32);
            return CryptographicOperations.FixedTimeEquals(computed, hash);
        }
    }
}