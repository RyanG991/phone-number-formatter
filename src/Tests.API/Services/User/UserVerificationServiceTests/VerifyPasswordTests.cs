using System;
using Xunit;

namespace PhoneNumberFormatter.Tests.API.Services.User.UserVerificationServiceTests
{
    public class VerifyPasswordTests : UserVerificationServiceTestBase
    {
        [Fact]
        public void ShouldReturnTrue_WhenPasswordMatches()
        {
            // Arrange
            var testUser = CreateTestUser(Guid.NewGuid(), "badUser", "password");

            // Act
            bool match = Sut.VerifyPassword("password", testUser);

            // Assert
            Assert.True(match);
        }

        [Fact]
        public void ShouldReturnFalse_WhenPasswordDoesNotMatch()
        {
            // Arrange
            var testUser = CreateTestUser(Guid.NewGuid(), "badUser", "password");

            // Act
            bool match = Sut.VerifyPassword("wrong", testUser);

            // Assert
            Assert.False(match);
        }


        [Fact]
        public void ShouldBubbleExceptions_WhenThrown()
        {
            // Arrange
            var testUser = CreateTestUser(Guid.NewGuid(), "badUser", "password");
            const string exceptionMessage = "Invalid!";

            PasswordHasherMock
                .Setup(ph => ph.HashPassword(testUser.HashedPassword, testUser.UserId.ToString()))
                .Throws(new InvalidOperationException(exceptionMessage));

            // Act
            Exception exception = Record.Exception(() => Sut.VerifyPassword("password", testUser));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }
    }
}
