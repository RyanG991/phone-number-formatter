namespace PhoneNumberFormatter.Hashing.Interfaces
{
    /// <summary>
    /// Exposes method(s) for the secure hashing of passwords
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Securely hashes a password
        /// </summary>
        string HashPassword(string password, string reference);
    }
}
