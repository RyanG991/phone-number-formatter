using Microsoft.Extensions.Logging;
using Moq;
using PhoneNumberFormatter.API.Services.User;
using PhoneNumberFormatter.Hashing.Interfaces;
using PhoneNumberFormatter.UserRepository.Interfaces;
using System;

namespace PhoneNumberFormatter.Tests.API.Services.User.UserVerificationServiceTests
{
    public abstract class UserVerificationServiceTestBase
    {
        protected UserVerificationService Sut;
        protected readonly Mock<IGetUserStore> GetUserStoreMock;
        protected readonly Mock<IPasswordHasher> PasswordHasherMock;
        protected readonly Mock<ILogger<UserVerificationService>> LoggerMock;

        protected const string UserName1 = "testUserName";

        protected UserVerificationServiceTestBase()
        {
            GetUserStoreMock = new Mock<IGetUserStore>();
            PasswordHasherMock = new Mock<IPasswordHasher>();
            LoggerMock = new Mock<ILogger<UserVerificationService>>();

            SetupGetUserStoreMock();
            SetupPasswordHasherMock();

            Sut = new UserVerificationService(
                    GetUserStoreMock.Object,
                    PasswordHasherMock.Object,
                    LoggerMock.Object
                );
        }

        protected virtual void SetupGetUserStoreMock()
        {
            GetUserStoreMock
                .Setup(gus => gus.GetByUserName(UserName1))
                .ReturnsAsync(new UserRepository.DTOs.User
                { 
                    UserId = Guid.NewGuid(), 
                    UserName = UserName1, 
                    HashedPassword = "hashedPassword" 
                });
        }

        protected virtual void SetupPasswordHasherMock()
        {
            PasswordHasherMock
                .Setup(ph => ph.HashPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string passord, string reference) => passord);
        }

        protected virtual UserRepository.DTOs.User CreateTestUser(Guid id, string userName, string hashedPassword)
            => new UserRepository.DTOs.User
            {
                UserId = id,
                UserName = userName,
                HashedPassword = hashedPassword
            };
    }
}
