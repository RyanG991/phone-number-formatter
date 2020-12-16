using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhoneNumberFormatter.API.Models.Errors;
using System;
using System.Net;

namespace PhoneNumberFormatter.API.Helpers.Controllers
{
    /// <summary>
    /// Contains common/universal functions for all controllers
    /// </summary>
    /// <remarks>TODO - move code into seperate logic class to facilitate unit testing</remarks>
    public class BaseApiController : ControllerBase
    {
        protected ILogger<BaseApiController> Logger;

        public BaseApiController(ILogger<BaseApiController> logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// Handles an exception, logging it and building an error response
        /// </summary>
        protected IActionResult HandleError(Exception exception)
        {
            // Log exception
            Logger.LogError(exception, "Error handled in controller.");

            switch (exception)
            {
                case ArgumentException:
                    return BuildErrorResponse(HttpStatusCode.BadRequest, exception.Message);
                default:
                    return InternalServerError(exception.Message);
            }
        }

        /// <summary>
        /// Builds an Internal Server Error response (500)
        /// </summary>
        protected IActionResult InternalServerError(string message)
            => BuildErrorResponse(HttpStatusCode.InternalServerError, message);

        private IActionResult BuildErrorResponse(HttpStatusCode code, string message)
            => StatusCode((int)code, new ErrorResponse(message));
    }
}
