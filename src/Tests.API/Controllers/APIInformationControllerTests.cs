using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PhoneNumberFormatter.API.Controllers;
using Xunit;

namespace PhoneNumberFormatter.Tests.API.Controllers
{
    public class APIInformationControllerTests
    {
        [Fact]
        public void Get_ShouldReturnOK_Normally()
        {
            // Arrange
            // Act
            IActionResult result = _sut.Get();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        private APIInformationController _sut;
        private readonly Mock<ILogger<APIInformationController>> _loggerMock;

        public APIInformationControllerTests()
        {
            _loggerMock = new Mock<ILogger<APIInformationController>>();
            _sut = new APIInformationController(_loggerMock.Object);
        }
    }
}
