using PhoneNumberFormatter.Hashing.Passwords;
using System;
using Xunit;

namespace PhoneNumberFormatter.Tests.Hashing.Passwords.PasswordHasherTests
{
    public class PasswordHasherTestBase
    {
        #region HashPassword
        [Fact]
        public void HashPassword_ShouldBcryptHash_Normally()
        {
            // Arrange
            // Act
            string result = _sut.HashPassword(Password1, Reference1);

            // Assert
            Assert.Equal(HashedPassword1, result);
        }

        [Fact]
        public void HashPassword_ShouldThrowArgumentException_WhenPasswordNull()
        {
            // Arrange
            // Act
            Exception exception = Record.Exception(() => _sut.HashPassword("", Reference1));

            // Assert
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void HashPassword_ShouldThrowArgumentException_WhenSaltNull()
        {
            // Arrange
            // Act
            Exception exception = Record.Exception(() => _sut.HashPassword(Password1, ""));

            // Assert
            Assert.IsType<ArgumentNullException>(exception);
        }
        #endregion

        #region Setup
        private PasswordHasher _sut;

        private const string Password1 = "T3st";
        private const string Reference1 = "1234";
        private const string HashedPassword1 = "$2a$13$......................xsfdfuKS7koTF7sOTDvYtZVokL6HDsK";

        public PasswordHasherTestBase()
        {
            _sut = new PasswordHasher();
        }
        #endregion
    }
}
