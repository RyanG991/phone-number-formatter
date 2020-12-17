using PhoneNumberFormatter.UserRepository.DTOs;
using PhoneNumberFormatter.UserRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneNumberFormatter.UserRepository.Stores
{
    public class StubGetUserStore : IGetUserStore
    {
        private readonly List<User> _allUsers;

        public StubGetUserStore()
        {
            _allUsers = new List<User>(GenerateTestUsers());
        }

        public async Task<User> GetByUserName(string userName)
        {
            await Task.CompletedTask;
            return _allUsers.FirstOrDefault(u => u.UserName == userName);
        }

        private IEnumerable<User> GenerateTestUsers()
        {
            // Basic auth for user: Basic dGVzdDpQYTU1dzByZA==
            yield return new User
            {
                UserId = Guid.Parse("7e18c550-51d6-454d-aaa7-a3376c722acd"),
                UserName = "test",
                // Password: Pa55w0rd
                HashedPassword = "$2a$13$......................ZseX9bk9J88.yZxnQXjyL2WFrecyA0i"
            };
        }
    }
}
