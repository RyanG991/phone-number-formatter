using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhoneNumberFormatter.API.Helpers.Controllers;
using PhoneNumberFormatter.API.Interfaces.Services.Formatting;
using PhoneNumberFormatter.API.Models.Errors;
using System;

namespace PhoneNumberFormatter.API.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class PhoneNumbersController : BaseApiController
    {
        private readonly IPhoneNumberFormattingService _formattingService;

        public PhoneNumbersController(
            IPhoneNumberFormattingService formattingService, 
            ILogger<PhoneNumbersController> logger) 
            : base(logger)
        {
            _formattingService = formattingService;
        }

        /// <summary>
        /// Prettifies a E.164 format phone number
        /// </summary>
        /// <response code="200">Returns the phone number in its prettified format</response>
        /// <response code="400">Invalid phone number supplied</response> 
        /// <response code="500">Unexpected error or issue with formatting</response>         
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [Route("{phoneNumber}/prettified")]
        public IActionResult PrettifyE164([FromRoute] string phoneNumber)
        {
            try
            {
                // Validate the request
                if (string.IsNullOrWhiteSpace(phoneNumber))
                    return ValidationErrorResponse($"{nameof(phoneNumber)} must have a value.");

                Logger.LogTrace($"Prettifying phone number: {phoneNumber}");

                // Prettify the number
                string prettified = _formattingService.PrettifyE164(phoneNumber);

                // Check the result
                if (string.IsNullOrWhiteSpace(prettified))
                    return InternalServerError($"Unexpected failure to prettify {phoneNumber}");

                // Build the OK response
                return Ok(prettified);                  
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
