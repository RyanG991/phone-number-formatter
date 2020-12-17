using PhoneNumberFormatter.FormattingRepository.Models;
using System.Collections.Generic;

namespace PhoneNumberFormatter.FormattingRepository.Interfaces
{
    /// <summary>
    /// Exposes method(s) for getting phone number formats
    /// </summary>
    public interface IPhoneNumberFormatsStore
    {
        /// <summary>
        /// Returns an ordered list of phone formats for a given country
        /// </summary>
        /// <param name="countryCode">Code used in E.164 for desired country, e.g. "44" for Great Britain</param>
        public List<E164Format> GetFormatsByCountry(string countryCode);
    }
}
