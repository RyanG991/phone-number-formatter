using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PhoneNumberFormatter.UserRepository.Contexts;
using PhoneNumberFormatter.UserRepository.DTOs;
using PhoneNumberFormatter.UserRepository.Stores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PhoneNumberFormatter.Tests.UserRepository.Stores
{
    public class GetUserStoreTests
    {
        #region GetByUserName
        [Theory]
        [InlineData(UserName1)]
        [InlineData(UserName2)]
        public async Task GetByUserName_ShouldReturnCorrectUser_Normally(string userName)
        {
            // Arrange
            // Act
            User result = await _sut.GetByUserName(userName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userName, result.UserName);
        }

        [Fact]
        public async Task GetByUserName_ShouldReturnNull_WhenNoUserFound()
        {
            // Arrange
            // Act
            User result = await _sut.GetByUserName("badUser");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByUserName_ShouldBubbleExceptions_WhenThrown()
        {
            // Arrange
            const string exceptionMessage = "Invalid!";

            _userContextFactoryMock
                .Setup(ucf => ucf.CreateDbContext())
                .Throws(new InvalidOperationException(exceptionMessage));

            // Act
            Exception exception = await Record.ExceptionAsync(() => _sut.GetByUserName("badUser"));

            // Assert
            Assert.IsType<InvalidOperationException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }
        #endregion

        #region Setup
        private readonly GetUserStore _sut;

        private readonly Mock<ILogger<GetUserStore>> _loggerMock;
        private readonly Mock<IDbContextFactory<UserContext>> _userContextFactoryMock;
        private readonly UserContext _userContext;

        public GetUserStoreTests()
        {
            _loggerMock = new Mock<ILogger<GetUserStore>>();
            _userContextFactoryMock = new Mock<IDbContextFactory<UserContext>>();

            var options = new DbContextOptionsBuilder<UserContext>()
                           .UseInMemoryDatabase(databaseName: "UserTestStore")
                           .Options;
            _userContext = new UserContext(options);

            SetupUserContextFactoryMock();

            _sut = new GetUserStore(_userContextFactoryMock.Object, _loggerMock.Object);
        }

        private void SetupUserContextFactoryMock()
        {
            List<User> mockUsers = AllUsers();

            // Build up in-memory database
            foreach (var user in mockUsers)
            {
                _userContext.Add(user);
            }
            _userContext.SaveChanges();

            _userContextFactoryMock
                .Setup(ucf => ucf.CreateDbContext())
                .Returns(_userContext);
        }
        #endregion

        #region Data
        private const string UserName1 = "testUser";
        private const string UserName2 = "anotherUser";


        private List<User> AllUsers()
            => new List<User>
            {
                new User {UserId = Guid.NewGuid(), UserName = UserName1},
                new User {UserId = Guid.NewGuid(), UserName = UserName2}
            };
        #endregion 
    }
}
