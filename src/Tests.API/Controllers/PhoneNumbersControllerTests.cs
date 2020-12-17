using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PhoneNumberFormatter.API.Controllers;
using PhoneNumberFormatter.API.Interfaces.Services.Formatting;
using System;
using Xunit;

namespace PhoneNumberFormatter.Tests.API.Controllers
{
    public class PhoneNumbersControllerTests
    {
        #region PrettifyE164
        [Fact]
        public void PrettifyE164_ShouldReturnOK_WhenPhoneNumberPrettified()
        {
            // Arrange
            // Act
            IActionResult result = _sut.PrettifyE164(GoodNumber);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void PrettifyE164_ShouldReturnStringContent_WhenPhoneNumberPrettified()
        {
            // Arrange
            // Act
            var result = _sut.PrettifyE164(GoodNumber) as OkObjectResult;

            // Assert
            Assert.Equal(GoodNumber, result.Value);
        }

        [Fact]
        public void PrettifyE164_ShouldReturn404_WhenPhoneNumberEmpty()
        {
            // Arrange
            // Act
            var result = _sut.PrettifyE164("") as ObjectResult;

            // Assert
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void PrettifyE164_ShouldReturn500_WhenErrorOccurs()
        {
            // Arrange
            const string throwsNumber = "+1011123345";

            _formattingServiceMock
                .Setup(fs => fs.PrettifyE164(throwsNumber))
                .Throws(new InvalidOperationException("Invalid!"));

            // Act
            var result = _sut.PrettifyE164(throwsNumber) as ObjectResult;

            // Assert
            Assert.Equal(500, result.StatusCode);
        }
        #endregion

        #region Setup
        private PhoneNumbersController _sut;
        private readonly Mock<IPhoneNumberFormattingService> _formattingServiceMock;
        private readonly Mock<ILogger<PhoneNumbersController>> _loggerMock;

        private const string GoodNumber = "+447174331622";
        private const string BadNumber = "+444174331622";

        public PhoneNumbersControllerTests()
        {
            _formattingServiceMock = new Mock<IPhoneNumberFormattingService>();
            _loggerMock = new Mock<ILogger<PhoneNumbersController>>();

            SetupFormattingServiceMock();

            _sut = new PhoneNumbersController(_formattingServiceMock.Object, _loggerMock.Object);
        }

        private void SetupFormattingServiceMock()
        {
            _formattingServiceMock
                .Setup(fs => fs.PrettifyE164(GoodNumber))
                .Returns(GoodNumber);

            _formattingServiceMock
                .Setup(fs => fs.PrettifyE164(BadNumber))
                .Returns(() => null);
        }
        #endregion
    }
}
