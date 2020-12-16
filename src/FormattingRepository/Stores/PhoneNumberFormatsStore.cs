using Microsoft.Extensions.Logging;
using PhoneNumberFormatter.FormattingRepository.Constants;
using PhoneNumberFormatter.FormattingRepository.Interfaces;
using PhoneNumberFormatter.FormattingRepository.Models;
using System;
using System.Collections.Generic;

namespace PhoneNumberFormatter.FormattingRepository.Stores
{
    /// <inheritdoc cref="IPhoneNumberFormatsStore"/>
    public class PhoneNumberFormatsStore : IPhoneNumberFormatsStore
    {
        private readonly ILogger<PhoneNumberFormatsStore> _logger;

        private Lazy<List<E164Format>> UKPhoneFormats;

        /// <inheritdoc cref="IPhoneNumberFormatsStore"/>
        public PhoneNumberFormatsStore(ILogger<PhoneNumberFormatsStore> logger)
        {
            UKPhoneFormats = new Lazy<List<E164Format>>(GenerateUKPhoneFormatsList);
            _logger = logger;
        }

        /// <inheritdoc />
        public List<E164Format> GetFormatsByCountry(string countryCode)
        {
            _logger.LogDebug($"Get phone formats for country: {countryCode}");

            switch (countryCode)
            {
                case CountryCodes.UK:
                    return UKPhoneFormats.Value;
                default:
                    throw new ArgumentException($"Country code ({countryCode}) is not yet supported.");
            }
        }

        private List<E164Format> GenerateUKPhoneFormatsList()
        {
            _logger.LogInformation("Generating UK phone formats list.");

            List<string> allUKFormats = AllUKPhoneFormats();

            List<E164Format> formatsList = new List<E164Format>();

            foreach (string format in allUKFormats)
            {
                // Remove whitespaces
                string cleanedFormat = format.Replace(" ", "");

                // Remove leading 0
                cleanedFormat = cleanedFormat.TrimStart('0');

                string matchingRegex = $"^{cleanedFormat.Replace("#", @"\d")}$";

                formatsList.Add(new E164Format
                {
                    Format = format,
                    MatchingRegex = matchingRegex
                });
            }

            return formatsList;
        }

        private List<string> AllUKPhoneFormats()
        => new List<string>
            {
                "09## ### ####",
                "08## ### ####",
                "0800 ######",
                "07### ######",
                "05### ######",
                "03## ### ####",
                "02# #### ####",
                "019756 #####",
                "019755 #####",
                "019467 #####",
                "017687 #####",
                "017684 #####",
                "017683 #####",
                "016977 #####",
                "016977 ####",
                "016974 #####",
                "016973 #####",
                "015396 #####",
                "015395 #####",
                "015394 #####",
                "015242 #####",
                "013873 #####",
                "013398 #####",
                "013397 #####",
                "01#1 ### ####",
                "011# ### ####",
                "01### ######",
                "01### #####"
            };
    }

}
