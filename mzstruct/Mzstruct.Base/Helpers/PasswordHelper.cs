using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Mzstruct.Base.Helpers
{
    public static class PasswordHelper
    {
        private static readonly int SaltSize = 16;
        private static readonly int HashSize = 32;
        private static readonly int Iterations = 100000;
        private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

        public static string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);
            return $"{Convert.ToHexString(hash)}-{Convert. ToHexString(salt)}";
        }

        public static bool Verify(string password, string passwordHash)
        {
            string[] parts = passwordHash. Split('-');
            byte[] hash = Convert.FromHexString(parts[0]);
            byte[] salt = Convert.FromHexString(parts[1]);
            byte[] inputHash = Rfc2898DeriveBytes. Pbkdf2(password, salt, Iterations, Algorithm, HashSize);
            //return hash.SequenceEqual(inputHash);
            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }

        public static string HashWithHMACSHA512(string password, out byte[] hash, out byte[] salt)
        {
            using (var hmac = new HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
            return $"{Convert.ToHexString(hash)}-{Convert. ToHexString(salt)}";
        }

        public static bool VerifyWithHMACSHA512(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
