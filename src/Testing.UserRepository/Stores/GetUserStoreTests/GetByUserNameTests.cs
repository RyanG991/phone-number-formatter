using PhoneNumberFormatter.UserRepository.DTOs;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PhoneNumberFormatter.Tests.UserRepository.Stores.GetUserStoreTests
{
    public class GetByUserNameTests : GetUserStoreTestBase
    {
        [Theory]
        [InlineData(UserName1)]
        [InlineData(UserName2)]
        public async Task ShouldReturnCorrectUser_Normally(string userName)
        {
            // Arrange
            // Act
            User result = await Sut.GetByUserName(userName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userName, result.UserName);
        }

        [Fact]
        public async Task ShouldReturnNull_WhenNoUserFound()
        {
            // Arrange
            // Act
            User result = await Sut.GetByUserName("badUser");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ShouldBubbleExceptions_WhenThrown()
        {
            // Arrange
            const string exceptionMessage = "Invalid!";

            UserContextFactoryMock
                .Setup(ucf => ucf.CreateDbContext())
                .Throws(new InvalidOperationException(exceptionMessage));

            // Act
            Exception exception = await Record.ExceptionAsync(() => Sut.GetByUserName("badUser"));

            // Assert
            Assert.IsType<InvalidOperationException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }
    }
}
