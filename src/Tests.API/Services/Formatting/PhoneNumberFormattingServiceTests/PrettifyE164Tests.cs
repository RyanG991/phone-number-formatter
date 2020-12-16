using System;
using Xunit;

namespace PhoneNumberFormatter.Tests.API.Services.Formatting.PhoneNumberFormattingServiceTests
{
    public class PrettifyE164Tests : PhoneNumberFormattingServiceTestBase
    {
        [Theory]
        [MemberData(nameof(TestPhoneNumbers))]
        public void ShouldPrettify_WhenValidUKNumber(string numberToTest, string expectedOutput)
        {
            // Arrange
            // Act
            string result = Sut.PrettifyE164(numberToTest);

            // Assert
            Assert.Equal(expectedOutput, result);
        }

        [Fact]
        public void ShouldThrowInvalidOperationException_WhenInvalidUKNumber()
        {
            // Arrange
            const string invalidUKNumber = "+4441231234";

            // Act
            Exception exception = Record.Exception(() => Sut.PrettifyE164(invalidUKNumber));

            // Assert
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ShouldThrowArgumentException_WhenNonUKNumber()
        {
            // Arrange
            const string nonUKNumber = "+14155552671";

            // Act
            Exception exception = Record.Exception(() => Sut.PrettifyE164(nonUKNumber));

            // Assert
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ShouldBubbleExceptions_WhenThrown()
        {
            // Arrange
            const string badCode = "44";
            const string invalidUKNumber = "+4441231234";
            const string exceptionMessage = "Invalid!";

            FormatStoreMock
                .Setup(fs => fs.GetFormatsByCountry(badCode))
                .Throws(new InvalidOperationException(exceptionMessage));

            // Act
            Exception exception = Record.Exception(() => Sut.PrettifyE164(invalidUKNumber));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }
    }
}
