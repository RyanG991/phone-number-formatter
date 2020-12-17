using PhoneNumberFormatter.API.Interfaces.Services.User;
using System.Threading.Tasks;
using PhoneNumberFormatter.UserRepository.Interfaces;
using PhoneNumberFormatter.Hashing.Interfaces;
using Microsoft.Extensions.Logging;

namespace PhoneNumberFormatter.API.Services.User
{
    using User = UserRepository.DTOs.User;

    /// <inheritdoc cref="IUserVerificationService"/>
    public class UserVerificationService : IUserVerificationService
    {
        private readonly IGetUserStore _getUserStore;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<UserVerificationService> _logger;

        /// <inheritdoc cref="IUserVerificationService"/>
        public UserVerificationService(
            IGetUserStore getUserStore,
            IPasswordHasher passwordHasher,
            ILogger<UserVerificationService> logger)
        {
            _getUserStore = getUserStore;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<(bool exists, User user)> UserExists(string userName)
        {
            User user = await _getUserStore.GetByUserName(userName);

            return new (user != null, user);
        }

        /// <inheritdoc />
        public bool VerifyPassword(string passwordToTest, User user)
        {
            _logger.LogTrace($"Verifying password for user: {user.UserId}");

            // First, hash the password we were sent
            string hashToTest = _passwordHasher.HashPassword(passwordToTest, user.UserId.ToString());

            return hashToTest == user.HashedPassword;
        }
    }
}
