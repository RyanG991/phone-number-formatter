using PhoneNumberFormatter.FormattingRepository.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace PhoneNumberFormatter.Tests.FormattingRepository.Stores.PhoneNumberFormatsStoreTests
{
    public class GetFormatsByCountryTests : PhoneNumberFormatsStoreTestBase
    {
        [Fact]
        public void ShouldReturnList_WhenUKCodeSupplied()
        {
            // Arrange
            // Act
            List<E164Format> result = Sut.GetFormatsByCountry(UKCountryCode);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(28, result.Count);
        }

        [Fact]
        public void ShouldThrowArgumentException_WhenCountryCodeNotSupported()
        {
            // Arrange
            // Act
            Exception exception = Record.Exception(() => Sut.GetFormatsByCountry("-99"));

            // Assert
            Assert.IsType<ArgumentException>(exception);
        }
    }
}
