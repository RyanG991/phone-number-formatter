using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using PhoneNumberFormatter.UserRepository.Contexts;
using PhoneNumberFormatter.UserRepository.DTOs;
using PhoneNumberFormatter.UserRepository.Interfaces;
using Microsoft.Extensions.Logging;

namespace PhoneNumberFormatter.UserRepository.Stores
{
    /// <inheritdoc cref="IGetUserStore"/>
    public class GetUserStore : IGetUserStore
    {
        private IDbContextFactory<UserContext> _userContextFactory;
        private readonly ILogger<GetUserStore> _logger;

        /// <inheritdoc cref="IGetUserStore"/>
        public GetUserStore(
            IDbContextFactory<UserContext> userContextFactory, 
            ILogger<GetUserStore> logger)
        {
            _userContextFactory = userContextFactory;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<User> GetByUserName(string userName)
        {
            _logger.LogTrace($"Getting user by username from database: {userName}");

            using (UserContext userDb = _userContextFactory.CreateDbContext())
            {
                IQueryable<User> userQuery = 
                    from u in userDb.Users
                    where u.UserName == userName
                    select u;

                User user = (await userQuery.ToListAsync()).FirstOrDefault();

                return user;
            }
        }
    }
}
