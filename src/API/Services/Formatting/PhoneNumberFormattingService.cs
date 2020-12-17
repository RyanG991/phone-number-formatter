using Microsoft.Extensions.Logging;
using PhoneNumberFormatter.API.Interfaces.Services.Formatting;
using PhoneNumberFormatter.FormattingRepository.Interfaces;
using PhoneNumberFormatter.FormattingRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PhoneNumberFormatter.API.Services.Formatting
{
    /// <inheritdoc cref="IPhoneNumberFormattingService" />
    public class PhoneNumberFormattingService : IPhoneNumberFormattingService
    {
        private readonly IPhoneNumberFormatsStore _formatStore;
        private readonly ILogger<PhoneNumberFormattingService> _logger;

        private const string _e164RegexPattern = @"^(\+?)(44)(\d{1,12})$";

        /// <inheritdoc cref="IPhoneNumberFormattingService" />
        public PhoneNumberFormattingService(
            IPhoneNumberFormatsStore formatStore, 
            ILogger<PhoneNumberFormattingService> logger)
        {
            _formatStore = formatStore;
            _logger = logger;
        }

        /// <inheritdoc />
        public string PrettifyE164(string phoneNumber)
        {
            _logger.LogDebug($"Prettifying phone number: {phoneNumber}");

            // Match the hone number with the E.164 format
            Match parsedNumber = MatchSingleE164Number(phoneNumber);

            // Extract the country code
            string countryCode = ExtractCountryCodeFromE164(parsedNumber);
            string subscriberNumber = ExtractSubscriberNumberFromE164(parsedNumber);

            _logger.LogDebug($"Getting pretty formats for country code: {countryCode}");

            // Get the list of pretty formats for the country code
            var prettyFormats = new List<E164Format>(_formatStore.GetFormatsByCountry(countryCode));

            // Find the matching format
            E164Format matchedFormat = FindMatchingFormat(subscriberNumber, prettyFormats);

            if (matchedFormat == null)
                throw new ArgumentException($"Invalid phone number supplied: {phoneNumber}");

            _logger.LogDebug($"Found matching format for phone number ({phoneNumber}): {matchedFormat.Format}");

            // Appply matched format
            string formattedNumber = FormatE164(subscriberNumber, matchedFormat);

            return formattedNumber;
        }

        private Match MatchSingleE164Number(string phoneNumber)
        {
            var e164Regex = new Regex(_e164RegexPattern);

            MatchCollection formatMatches = e164Regex.Matches(phoneNumber);

            if (formatMatches.Count == 0)
                throw new ArgumentException($"Phone number ({phoneNumber}) is not in supported E.164 format.");
            if (formatMatches.Count > 1)
                throw new ArgumentException("Cannot format more than one phone number at a time.");

            return formatMatches[0];
        }

        /// <summary>
        /// Extracts the country code from a regex match on phone number
        /// </summary>
        private string ExtractCountryCodeFromE164(Match match)
            => match.Groups[2].Value;

        /// <summary>
        /// Extracts the subscriber from a regex match on phone number
        /// </summary>
        private string ExtractSubscriberNumberFromE164(Match match)
            => match.Groups[3].Value;

        /// <summary>
        /// Finds a matching format for the phone number
        /// </summary>
        private E164Format FindMatchingFormat(string subscriberNumber, List<E164Format> prettyFormats)
        {
            // Quickly remove formats where the first number doesn't match
            var firstPassFormats = 
                prettyFormats
                    .Where(pf => pf.Format[1] == subscriberNumber[0] || pf.Format[1] == '#')
                    .ToList();

            if (firstPassFormats.Count == 0)
            {
                return null;
            }

            // Loop through remaining formats until we find a match
            foreach (E164Format format in firstPassFormats)
            {
                // Build regex for matching subscriber to format
                // TODO - make Regex class part of repository response for efficiency
                var matchingRegex = new Regex(format.MatchingRegex);

                // If it doesn't match, continue the search
                if (!matchingRegex.IsMatch(subscriberNumber))
                    continue;

                return format;
            }

            return null;
        }

        /// <summary>
        /// Applys a pretty format to a phone number
        /// </summary>
        private string FormatE164(string subscriberNumber, E164Format format)
        {
            var formattedNumberBuilder = new StringBuilder();
            int subscriberIndex = 0;

            for (int i = 0; i < format.Format.Length; i++)
                {
                var currentFormatCharacter = format.Format[i];

                // Start with the leading 0, if there
                if (currentFormatCharacter == '0' && i == 0)
                {
                    formattedNumberBuilder.Append(currentFormatCharacter);
                }
                // Or, if format has a space, just add it
                else if (currentFormatCharacter == ' ')
                {
                    formattedNumberBuilder.Append(' ');
                }
                // Otherwise, just add the subscriber character we have got to
                else
                {
                    formattedNumberBuilder.Append(subscriberNumber[subscriberIndex]);
                    subscriberIndex++;
                }
            }

            return formattedNumberBuilder.ToString();
        }
    }
}
