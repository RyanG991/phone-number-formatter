using PhoneNumberFormatter.Hashing.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;

namespace PhoneNumberFormatter.Hashing.Passwords
{
    /// <inheritdoc cref="IPasswordHasher"/>
    public class PasswordHasher : IPasswordHasher
    {
        /// <inheritdoc />
        public string HashPassword(string password, string reference)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException($"{nameof(password)} must not be null or whitespace.");
            if (string.IsNullOrWhiteSpace(reference))
                throw new ArgumentNullException($"{nameof(reference)} must not be null or whitespace.");

            // Generate a salt for this password
            string salt = GenerateSalt(reference);

            // Slow-hash the password
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        /// <summary>
        /// Generates a secure salt by combining the public and private salts.
        /// This results in secure, predictable and unique password hashes
        /// </summary>
        private string GenerateSalt(string publicSalt)
        {
            // Build the salt by combinging private and public salts
            string preSalt = $"8DD709B5-7795-4569-{publicSalt}-8AE5-1930443B6537";

            // Encode the string into bytes
            byte[] data = Encoding.UTF8.GetBytes(preSalt);

            // Quick-hash the bytes
            SHA512 shaM = new SHA512Managed();
            byte[]  result = shaM.ComputeHash(data);

            // Decode hashed string
            var saltPart = Encoding.UTF8.GetString(result);

            return $"$2a$13${saltPart}";
        }
    }
}
