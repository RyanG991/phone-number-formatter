using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PhoneNumberFormatter.Tests.Hashing.Passwords.PasswordHasherTests
{
    public class HashPasswordTests : PasswordHasherTestBase
    {
        [Fact]
        public void ShouldBcryptHash_Normally()
        {
            // Arrange
            // Act
            string result = Sut.HashPassword(Password1, Reference1);

            // Assert
            Assert.Equal(HashedPassword1, result);
        }

        [Fact]
        public void ShouldThrowArgumentException_WhenPasswordNull()
        {
            // Arrange
            // Act
            Exception exception = Record.Exception(() => Sut.HashPassword("", Reference1));

            // Assert
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ShouldThrowArgumentException_WhenSaltNull()
        {
            // Arrange
            // Act
            Exception exception = Record.Exception(() => Sut.HashPassword(Password1, ""));

            // Assert
            Assert.IsType<ArgumentNullException>(exception);
        }
    }
}
