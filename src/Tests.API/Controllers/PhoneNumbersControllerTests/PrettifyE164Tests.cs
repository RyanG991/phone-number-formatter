using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace PhoneNumberFormatter.Tests.API.Controllers.PhoneNumbersControllerTests
{
    public class PrettifyE164Tests : PhoneNumbersControllerTestBase
    {
        [Fact]
        public void ShouldReturnOK_WhenPhoneNumberPrettified()
        {
            // Arrange
            // Act
            IActionResult result = Sut.PrettifyE164(GoodNumber);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void ShouldReturnStringContent_WhenPhoneNumberPrettified()
        {
            // Arrange
            // Act
            var result = Sut.PrettifyE164(GoodNumber) as OkObjectResult;

            // Assert
            Assert.Equal(GoodNumber, result.Value);
        }

        [Fact]
        public void ShouldReturn404_WhenPhoneNumberEmpty()
        {
            // Arrange
            // Act
            var result = Sut.PrettifyE164("") as ObjectResult;

            // Assert
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void ShouldReturn500_WhenErrorOccurs()
        {
            // Arrange
            const string throwsNumber = "+1011123345";

            FormattingServiceMock
                .Setup(fs => fs.PrettifyE164(throwsNumber))
                .Throws(new InvalidOperationException("Invalid!"));

            // Act
            var result = Sut.PrettifyE164(throwsNumber) as ObjectResult;

            // Assert
            Assert.Equal(500, result.StatusCode);
        }
    }
}
