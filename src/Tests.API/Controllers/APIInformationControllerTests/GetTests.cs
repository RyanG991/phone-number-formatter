using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace PhoneNumberFormatter.Tests.API.Controllers.APIInformationControllerTests
{
    public class GetTests : APIInformationControllerTestBase
    {
        [Fact]
        public void ShouldReturnOK_Normally()
        {
            // Arrange
            // Act
            IActionResult result = Sut.Get();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
