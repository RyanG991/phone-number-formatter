using Microsoft.Extensions.Logging;
using Moq;
using PhoneNumberFormatter.FormattingRepository.Models;
using PhoneNumberFormatter.FormattingRepository.Stores;
using System;
using System.Collections.Generic;
using Xunit;

namespace PhoneNumberFormatter.Tests.FormattingRepository.Stores
{
    public class PhoneNumberFormatsStoreTests
    {
        [Fact]
        public void GetFormatsByCountry_ShouldReturnList_WhenUKCodeSupplied()
        {
            // Arrange
            // Act
            List<E164Format> result = _sut.GetFormatsByCountry(UKCountryCode);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(28, result.Count);
        }

        [Fact]
        public void GetFormatsByCountry_ShouldThrowArgumentException_WhenCountryCodeNotSupported()
        {
            // Arrange
            // Act
            Exception exception = Record.Exception(() => _sut.GetFormatsByCountry("-99"));

            // Assert
            Assert.IsType<ArgumentException>(exception);
        }



        private PhoneNumberFormatsStore _sut;
        private readonly Mock<ILogger<PhoneNumberFormatsStore>> _loggerMock;

        private const string UKCountryCode = "44";

        public PhoneNumberFormatsStoreTests()
        {
            _loggerMock = new Mock<ILogger<PhoneNumberFormatsStore>>();
            _sut = new PhoneNumberFormatsStore(_loggerMock.Object);
        }
    }
}
