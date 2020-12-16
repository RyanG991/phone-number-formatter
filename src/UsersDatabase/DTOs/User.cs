using System;
using System.ComponentModel.DataAnnotations;

namespace PhoneNumberFormatter.UserRepository.DTOs
{
    /// <summary>
    /// Our basic user of the API
    /// </summary>
    public class User
    {
        public User() { }

        [Key]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string UserName { get; set; }

        [Required]
        public string HashedPassword { get; set; }
    }
}
