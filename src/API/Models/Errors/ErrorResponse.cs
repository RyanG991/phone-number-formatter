using System;

namespace PhoneNumberFormatter.API.Models.Errors
{
    public class ErrorResponse
    {
        public ErrorResponse(Guid requestId, string message)
        {
            RequestId = requestId;
            Message = message;
        }

        public ErrorResponse(string message)
        {
            RequestId = Guid.Empty;
            Message = message;
        }

        public Guid RequestId { get; set; }
        public string Message { get; set; }
    }
}
