using Microsoft.Extensions.Logging;
using Moq;
using PhoneNumberFormatter.API.Services.Formatting;
using PhoneNumberFormatter.FormattingRepository.Interfaces;
using PhoneNumberFormatter.FormattingRepository.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace PhoneNumberFormatter.Tests.API.Services.Formatting
{
    public class PhoneNumberFormattingServiceTests
    {
        #region PrettifyE164
        [Theory]
        [MemberData(nameof(TestPhoneNumbers))]
        public void PrettifyE164_ShouldPrettify_WhenValidUKNumber(string numberToTest, string expectedOutput)
        {
            // Arrange
            // Act
            string result = _sut.PrettifyE164(numberToTest);

            // Assert
            Assert.Equal(expectedOutput, result);
        }

        [Theory]
        [MemberData(nameof(InvalidUKPhoneNumbers))]
        public void PrettifyE164_ShouldThrowInvalidOperationException_WhenInvalidUKNumber(string invalidUKNumber)
        {
            // Arrange
            // Act
            Exception exception = Record.Exception(() => _sut.PrettifyE164(invalidUKNumber));

            // Assert
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void PrettifyE164_ShouldThrowArgumentException_WhenNonUKNumber()
        {
            // Arrange
            const string nonUKNumber = "+14155552671";

            // Act
            Exception exception = Record.Exception(() => _sut.PrettifyE164(nonUKNumber));

            // Assert
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void PrettifyE164_ShouldBubbleExceptions_WhenThrown()
        {
            // Arrange
            const string badCode = "44";
            const string invalidUKNumber = "+4441231234";
            const string exceptionMessage = "Invalid!";

            _formatStoreMock
                .Setup(fs => fs.GetFormatsByCountry(badCode))
                .Throws(new InvalidOperationException(exceptionMessage));

            // Act
            Exception exception = Record.Exception(() => _sut.PrettifyE164(invalidUKNumber));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }
        #endregion

        #region Setup
        private PhoneNumberFormattingService _sut;
        private readonly Mock<IPhoneNumberFormatsStore> _formatStoreMock;
        private readonly Mock<ILogger<PhoneNumberFormattingService>> _loggerMock;

        public PhoneNumberFormattingServiceTests()
        {
            _formatStoreMock = new Mock<IPhoneNumberFormatsStore>();
            _loggerMock = new Mock<ILogger<PhoneNumberFormattingService>>();

            SetupFormatStoreMock();

            _sut = new PhoneNumberFormattingService(_formatStoreMock.Object, _loggerMock.Object);
        }

        private void SetupFormatStoreMock()
        {
            _formatStoreMock
                .Setup(fs => fs.GetFormatsByCountry("44"))
                .Returns(UKFormats);
        }
        #endregion

        #region Data
        /// <summary>
        /// Test phone numbers should be in form (test, expectedOutcome)
        /// </summary>
        public static IEnumerable<object[]> TestPhoneNumbers =>
            new List<object[]>
            {
                new object[] { "+44195963867", "01959 63867" },
                new object[] { "+441959638677", "01959 638677" },
                new object[] { "+441195963867", "0119 596 3867" },
                new object[] { "+441915963867", "0191 596 3867" },
                new object[] { "+441339795963", "013397 95963" },
                new object[] { "+441339895963", "013398 95963" },
                new object[] { "+441387395963", "013873 95963" },
                new object[] { "+441524295963", "015242 95963" },
                new object[] { "+441539495963", "015394 95963" },
                new object[] { "+441539595963", "015395 95963" },
                new object[] { "+441539695963", "015396 95963" },
                new object[] { "+441697395963", "016973 95963" },
                new object[] { "+441697495963", "016974 95963" },
                new object[] { "+44169779596", "016977 9596" },
                new object[] { "+441697795963", "016977 95963" },
                new object[] { "+441768395963", "017683 95963" },
                new object[] { "+441768495963", "017684 95963" },
                new object[] { "+441768795963", "017687 95963" },
                new object[] { "+441946795963", "019467 95963" },
                new object[] { "+441975595963", "019755 95963" },
                new object[] { "+441975695963", "019756 95963" },
                new object[] { "+442959638677", "029 5963 8677" },
                new object[] { "+443959638677", "0395 963 8677" },
                new object[] { "+445959638677", "05959 638677" },
                new object[] { "+447959638677", "07959 638677" },
                new object[] { "+44800959638", "0800 959638" },
                new object[] { "+448959638677", "0895 963 8677" },
                new object[] { "+449959638677", "0995 963 8677" }
            };

        public static List<E164Format> UKFormats =>
            new List<E164Format>
            {
                new E164Format { Format = "09## ### ####", MatchingRegex = @"^9\d\d\d\d\d\d\d\d\d$" },
                new E164Format { Format = "08## ### ####", MatchingRegex = @"^8\d\d\d\d\d\d\d\d\d$" },
                new E164Format { Format = "0800 ######", MatchingRegex = @"^800\d\d\d\d\d\d$" },
                new E164Format { Format = "07### ######", MatchingRegex = @"^7\d\d\d\d\d\d\d\d\d$" },
                new E164Format { Format = "05### ######", MatchingRegex = @"^5\d\d\d\d\d\d\d\d\d$" },
                new E164Format { Format = "03## ### ####", MatchingRegex = @"^3\d\d\d\d\d\d\d\d\d$" },
                new E164Format { Format = "02# #### ####", MatchingRegex = @"^2\d\d\d\d\d\d\d\d\d$" },
                new E164Format { Format = "019756 #####", MatchingRegex = @"^19756\d\d\d\d\d$" },
                new E164Format { Format = "019755 #####", MatchingRegex = @"^19755\d\d\d\d\d$" },
                new E164Format { Format = "019467 #####", MatchingRegex = @"^19467\d\d\d\d\d$" },
                new E164Format { Format = "017687 #####", MatchingRegex = @"^17687\d\d\d\d\d$" },
                new E164Format { Format = "017684 #####", MatchingRegex = @"^17684\d\d\d\d\d$" },
                new E164Format { Format = "017683 #####", MatchingRegex = @"^17683\d\d\d\d\d$" },
                new E164Format { Format = "016977 #####", MatchingRegex = @"^16977\d\d\d\d\d$" },
                new E164Format { Format = "016977 ####", MatchingRegex = @"^16977\d\d\d\d$" },
                new E164Format { Format = "016974 #####", MatchingRegex = @"^16974\d\d\d\d\d$" },
                new E164Format { Format = "016973 #####", MatchingRegex = @"^16973\d\d\d\d\d$" },
                new E164Format { Format = "015396 #####", MatchingRegex = @"^15396\d\d\d\d\d$" },
                new E164Format { Format = "015395 #####", MatchingRegex = @"^15395\d\d\d\d\d$" },
                new E164Format { Format = "015394 #####", MatchingRegex = @"^15394\d\d\d\d\d$" },
                new E164Format { Format = "015242 #####", MatchingRegex = @"^15242\d\d\d\d\d$" },
                new E164Format { Format = "013873 #####", MatchingRegex = @"^13873\d\d\d\d\d$" },
                new E164Format { Format = "013398 #####", MatchingRegex = @"^13398\d\d\d\d\d$" },
                new E164Format { Format = "013397 #####", MatchingRegex = @"^13397\d\d\d\d\d$" },
                new E164Format { Format = "01#1 ### ####", MatchingRegex = @"^1\d1\d\d\d\d\d\d\d$" },
                new E164Format { Format = "011# ### ####", MatchingRegex = @"^11\d\d\d\d\d\d\d\d$" },
                new E164Format { Format = "01### ######", MatchingRegex = @"^1\d\d\d\d\d\d\d\d\d$" },
                new E164Format { Format = "01### #####", MatchingRegex = @"^1\d\d\d\d\d\d\d\d$" }
            };

        /// <summary>
        /// Invalid phone numbers
        /// </summary>
        public static IEnumerable<object[]> InvalidUKPhoneNumbers =>
            new List<object[]>
            {
                new object[] { "+4441231234" },
                new object[] { "+4419596386" },
                new object[] { "+4411959638" },
                new object[] { "+4419159638" },
                new object[] { "+4413397959" },
                new object[] { "+4413398959" },
                new object[] { "+4413873959" },
                new object[] { "+4415242959" },
                new object[] { "+4415394959" },
                new object[] { "+4415395959" },
                new object[] { "+4415396959" },
                new object[] { "+4416973959" },
                new object[] { "+4416974959" },
                new object[] { "+4416977959" },
                new object[] { "+4416977959" },
                new object[] { "+4417683959" },
                new object[] { "+4417684959" },
                new object[] { "+4417687959" },
                new object[] { "+4419467959" },
                new object[] { "+4419755959" },
                new object[] { "+4419756959" },
                new object[] { "+4429596386" },
                new object[] { "+4439596386" },
                new object[] { "+4459596386" },
                new object[] { "+4479596386" },
                new object[] { "+4480095963" },
                new object[] { "+4489596386" },
                new object[] { "+4499596386" }
            };
        #endregion
    }
}
