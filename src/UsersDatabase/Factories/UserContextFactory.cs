using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using PhoneNumberFormatter.UserRepository.Contexts;

namespace PhoneNumberFormatter.UserRepository.Factories
{
    public class UserContextFactory : IDesignTimeDbContextFactory<UserContext>
    {
        public UserContextFactory()
        {
        }

        private IConfiguration Configuration 
            => new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

        public UserContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<UserContext>();
            builder.UseSqlServer(Configuration.GetConnectionString("main"));

            return new UserContext(builder.Options);
        }
    }
}
