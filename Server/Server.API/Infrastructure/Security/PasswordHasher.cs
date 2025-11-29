using Server.API.Application.Abstractions;
using System.Security.Cryptography;

namespace Server.API.Infrastructure.Security
{
    /// <summary>
    /// Simple PBKDF2-based password hasher.
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        private const int _saltSize = 16; // 128 bit
        private const int _keySize = 32;  // 256 bit
        // IMPORTANT: iterations aren't stored in the database. Never change this value without applying the changes to a users table.
        private const int _iterations = 100_000; 

        public void HashPassword(string password, out byte[] hash, out byte[] salt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password must not be empty.", nameof(password));

            salt = RandomNumberGenerator.GetBytes(_saltSize);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, _iterations, HashAlgorithmName.SHA256);
            hash = pbkdf2.GetBytes(_keySize);
        }

        public bool VerifyPassword(string password, byte[] hash, byte[] salt)
        {
            if (hash is null) throw new ArgumentNullException(nameof(hash));
            if (salt is null) throw new ArgumentNullException(nameof(salt));

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, _iterations, HashAlgorithmName.SHA256);
            var computed = pbkdf2.GetBytes(_keySize);

            return CryptographicOperations.FixedTimeEquals(hash, computed);
        }

    }
}
