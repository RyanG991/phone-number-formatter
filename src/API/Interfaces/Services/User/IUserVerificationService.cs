using System.Threading.Tasks;

namespace PhoneNumberFormatter.API.Interfaces.Services.User
{
    using User = UserRepository.DTOs.User;

    /// <summary>
    /// Exposes method(s) for verifying user data
    /// </summary>
    public interface IUserVerificationService
    {
        /// <summary>
        /// Checks whether a user name exists in our database and returns the found user.
        /// </summary>
        Task<(bool exists, User user)> UserExists(string userName);

        /// <summary>
        /// Verifies a user's password against one sent to the API
        /// </summary>
        bool VerifyPassword(string passwordToTest, User user);
    }
}
