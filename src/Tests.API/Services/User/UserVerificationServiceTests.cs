using Microsoft.Extensions.Logging;
using Moq;
using PhoneNumberFormatter.API.Services.User;
using PhoneNumberFormatter.Hashing.Interfaces;
using PhoneNumberFormatter.UserRepository.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PhoneNumberFormatter.Tests.API.Services.User
{
    public class UserVerificationServiceTests
    {
        #region UserExists
        [Fact]
        public async Task UserExists_ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            // Act
            (bool exists, UserRepository.DTOs.User user) = await _sut.UserExists(UserName1);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task UserExists_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            // Act
            (bool exists, UserRepository.DTOs.User user) = await _sut.UserExists(UserName1);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(UserName1, user.UserName);
        }

        [Fact]
        public async Task UserExists_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            // Act
            (bool exists, UserRepository.DTOs.User user) = await _sut.UserExists("badUser");

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task UserExists_ShouldReturnNullUser_WhenUserDoesNotExist()
        {
            // Arrange
            // Act
            (bool exists, UserRepository.DTOs.User user) = await _sut.UserExists("badUser");

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public async Task UserExists_ShouldBubbleExceptions_WhenThrown()
        {
            // Arrange
            const string userName = "throw";
            const string exceptionMessage = "Invalid!";

            _getUserStoreMock
                .Setup(gus => gus.GetByUserName(userName))
                .Throws(new InvalidOperationException(exceptionMessage));

            // Act
            Exception exception = await Record.ExceptionAsync(() => _sut.UserExists(userName));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }
        #endregion


        #region VerifyPassword
        [Fact]
        public void VerifyPassword_ShouldReturnTrue_WhenPasswordMatches()
        {
            // Arrange
            var testUser = CreateTestUser(Guid.NewGuid(), "badUser", "password");

            // Act
            bool match = _sut.VerifyPassword("password", testUser);

            // Assert
            Assert.True(match);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_WhenPasswordDoesNotMatch()
        {
            // Arrange
            var testUser = CreateTestUser(Guid.NewGuid(), "badUser", "password");

            // Act
            bool match = _sut.VerifyPassword("wrong", testUser);

            // Assert
            Assert.False(match);
        }


        [Fact]
        public void VerifyPassword_ShouldBubbleExceptions_WhenThrown()
        {
            // Arrange
            var testUser = CreateTestUser(Guid.NewGuid(), "badUser", "password");
            const string exceptionMessage = "Invalid!";

            _passwordHasherMock
                .Setup(ph => ph.HashPassword(testUser.HashedPassword, testUser.UserId.ToString()))
                .Throws(new InvalidOperationException(exceptionMessage));

            // Act
            Exception exception = Record.Exception(() => _sut.VerifyPassword("password", testUser));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }
        #endregion


        #region Setup
        private UserVerificationService _sut;
        private readonly Mock<IGetUserStore> _getUserStoreMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<ILogger<UserVerificationService>> _loggerMock;

        private const string UserName1 = "testUserName";

        public UserVerificationServiceTests()
        {
            _getUserStoreMock = new Mock<IGetUserStore>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _loggerMock = new Mock<ILogger<UserVerificationService>>();

            SetupGetUserStoreMock();
            SetupPasswordHasherMock();

            _sut = new UserVerificationService(
                    _getUserStoreMock.Object,
                    _passwordHasherMock.Object,
                    _loggerMock.Object
                );
        }

        private void SetupGetUserStoreMock()
        {
            _getUserStoreMock
                .Setup(gus => gus.GetByUserName(UserName1))
                .ReturnsAsync(new UserRepository.DTOs.User
                { 
                    UserId = Guid.NewGuid(), 
                    UserName = UserName1, 
                    HashedPassword = "hashedPassword" 
                });
        }

        private void SetupPasswordHasherMock()
        {
            _passwordHasherMock
                .Setup(ph => ph.HashPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string passord, string reference) => passord);
        }
        #endregion

        private UserRepository.DTOs.User CreateTestUser(Guid id, string userName, string hashedPassword)
            => new UserRepository.DTOs.User
            {
                UserId = id,
                UserName = userName,
                HashedPassword = hashedPassword
            };
    }
}
