namespace PhoneNumberFormatter.API.Interfaces.Services.Formatting
{
    /// <summary>
    /// Exposes method(s) for formatting phone numbers
    /// </summary>
    public interface IPhoneNumberFormattingService
    {
        /// <summary>
        /// Prettify (format) and E.164 formatted number
        /// </summary>
        public string PrettifyE164(string phoneNUmber);
    }
}
