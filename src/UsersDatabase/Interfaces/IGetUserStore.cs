using System.Threading.Tasks;
using PhoneNumberFormatter.UserRepository.DTOs;

namespace PhoneNumberFormatter.UserRepository.Interfaces
{
    /// <summary>
    /// Exposes method(s) for getting users
    /// </summary>
    public interface IGetUserStore
    {
        /// <summary>
        /// Get an individual user by their user name
        /// </summary>
        Task<User> GetByUserName(string userName);
    } 
}
