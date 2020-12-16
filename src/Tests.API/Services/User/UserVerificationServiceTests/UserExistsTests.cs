using System;
using System.Threading.Tasks;
using Xunit;

namespace PhoneNumberFormatter.Tests.API.Services.User.UserVerificationServiceTests
{
    public class UserExistsTests : UserVerificationServiceTestBase
    {
        [Fact]
        public async Task ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            // Act
            (bool exists, UserRepository.DTOs.User user) = await Sut.UserExists(UserName1);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            // Act
            (bool exists, UserRepository.DTOs.User user) = await Sut.UserExists(UserName1);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(UserName1, user.UserName);
        }

        [Fact]
        public async Task ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            // Act
            (bool exists, UserRepository.DTOs.User user) = await Sut.UserExists("badUser");

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task ShouldReturnNullUser_WhenUserDoesNotExist()
        {
            // Arrange
            // Act
            (bool exists, UserRepository.DTOs.User user) = await Sut.UserExists("badUser");

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public async Task ShouldBubbleExceptions_WhenThrown()
        {
            // Arrange
            const string userName = "throw";
            const string exceptionMessage = "Invalid!";

            GetUserStoreMock
                .Setup(gus => gus.GetByUserName(userName))
                .Throws(new InvalidOperationException(exceptionMessage));

            // Act
            Exception exception =  await Record.ExceptionAsync(() => Sut.UserExists(userName));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }
    }
}
