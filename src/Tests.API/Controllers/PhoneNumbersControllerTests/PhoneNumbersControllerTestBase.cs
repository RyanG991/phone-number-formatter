using Microsoft.Extensions.Logging;
using Moq;
using PhoneNumberFormatter.API.Controllers;
using PhoneNumberFormatter.API.Interfaces.Services.Formatting;

namespace PhoneNumberFormatter.Tests.API.Controllers.PhoneNumbersControllerTests
{
    public abstract class PhoneNumbersControllerTestBase
    {
        protected PhoneNumbersController Sut;
        protected readonly Mock<IPhoneNumberFormattingService> FormattingServiceMock;
        protected readonly Mock<ILogger<PhoneNumbersController>> LoggerMock;

        protected const string GoodNumber = "+447174331622";
        protected const string BadNumber = "+444174331622";

        protected PhoneNumbersControllerTestBase()
        {
            FormattingServiceMock = new Mock<IPhoneNumberFormattingService>();
            LoggerMock = new Mock<ILogger<PhoneNumbersController>>();

            SetupFormattingServiceMock();

            Sut = new PhoneNumbersController(FormattingServiceMock.Object, LoggerMock.Object);
        }

        protected virtual void SetupFormattingServiceMock()
        {
            FormattingServiceMock
                .Setup(fs => fs.PrettifyE164(GoodNumber))
                .Returns(GoodNumber);

            FormattingServiceMock
                .Setup(fs => fs.PrettifyE164(BadNumber))
                .Returns(() => null);
        }
    }
}
