using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PhoneNumberFormatter.UserRepository.Contexts;
using PhoneNumberFormatter.UserRepository.DTOs;
using PhoneNumberFormatter.UserRepository.Stores;
using System;
using System.Collections.Generic;

namespace PhoneNumberFormatter.Tests.UserRepository.Stores.GetUserStoreTests
{
    public abstract class GetUserStoreTestBase
    {
        protected GetUserStore Sut;

        protected readonly Mock<ILogger<GetUserStore>> LoggerMock;
        protected readonly Mock<IDbContextFactory<UserContext>> UserContextFactoryMock;
        protected readonly UserContext UserContext;

        protected const string UserName1 = "testUser";
        protected const string UserName2 = "anotherUser";

        protected GetUserStoreTestBase()
        {
            LoggerMock = new Mock<ILogger<GetUserStore>>();
            UserContextFactoryMock = new Mock<IDbContextFactory<UserContext>>();

            var options = new DbContextOptionsBuilder<UserContext>()
                           .UseInMemoryDatabase(databaseName: "UserTestStore")
                           .Options;
            UserContext = new UserContext(options);

            SetupUserContextFactoryMock();

            Sut = new GetUserStore(UserContextFactoryMock.Object, LoggerMock.Object);
        }

        protected virtual void SetupUserContextFactoryMock()
        {
            List<User> mockUsers = AllUsers();

            // Build up in-memory database
            foreach (var user in mockUsers)
            {
                UserContext.Add(user);
            }
            UserContext.SaveChanges();

            UserContextFactoryMock
                .Setup(ucf => ucf.CreateDbContext())
                .Returns(UserContext);
        }

        protected virtual List<User> AllUsers()
            => new List<User>
            {
                new User {UserId = Guid.NewGuid(), UserName = UserName1},
                new User {UserId = Guid.NewGuid(), UserName = UserName2}
            };
    }
}
