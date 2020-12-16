namespace PhoneNumberFormatter.FormattingRepository.Models
{
    public class E164Format
    {
        /// <summary>
        /// String format with hashes, e.g. 01### #####
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Regex used to match against an E.164 number
        /// </summary>
        public string MatchingRegex { get; set; }
    }
}
