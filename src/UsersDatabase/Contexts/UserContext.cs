using Microsoft.EntityFrameworkCore;
using PhoneNumberFormatter.UserRepository.DTOs;

namespace PhoneNumberFormatter.UserRepository.Contexts
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        public UserContext() : base() { }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique(true);
        }
    }
}
