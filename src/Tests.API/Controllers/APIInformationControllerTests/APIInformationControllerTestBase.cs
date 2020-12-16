using Microsoft.Extensions.Logging;
using Moq;
using PhoneNumberFormatter.API.Controllers;

namespace PhoneNumberFormatter.Tests.API.Controllers.APIInformationControllerTests
{
    public abstract class APIInformationControllerTestBase
    {
        protected APIInformationController Sut;
        protected readonly Mock<ILogger<APIInformationController>> LoggerMock;

        protected APIInformationControllerTestBase()
        {
            LoggerMock = new Mock<ILogger<APIInformationController>>();
            Sut = new APIInformationController(LoggerMock.Object);
        }
    }
}
