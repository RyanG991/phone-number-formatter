using Microsoft.Extensions.Logging;
using Moq;
using PhoneNumberFormatter.FormattingRepository.Stores;

namespace PhoneNumberFormatter.Tests.FormattingRepository.Stores.PhoneNumberFormatsStoreTests
{
    public abstract class PhoneNumberFormatsStoreTestBase
    {
        protected PhoneNumberFormatsStore Sut;
        protected readonly Mock<ILogger<PhoneNumberFormatsStore>> LoggerMock;

        protected const string UKCountryCode = "44";

        protected PhoneNumberFormatsStoreTestBase()
        {
            LoggerMock = new Mock<ILogger<PhoneNumberFormatsStore>>();
            Sut = new PhoneNumberFormatsStore(LoggerMock.Object);
        }
    }
}
