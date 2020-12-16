using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhoneNumberFormatter.API.Helpers.Controllers;
using PhoneNumberFormatter.API.Models.Errors;
using PhoneNumberFormatter.API.Models.Information;
using System;
using System.Reflection;

namespace PhoneNumberFormatter.API.Controllers
{
    /// <summary>
    /// Provides information about the running API
    /// </summary>
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    [AllowAnonymous]
    public class APIInformationController : BaseApiController
    {
        public APIInformationController(ILogger<APIInformationController> logger) : base(logger)
        {
        }

        /// <summary>
        /// Returns the basic API information, e.g. version
        /// </summary>
        /// <response code="200">Returns the basic API information, e.g. version</response>
        /// <response code="500">Unexpected server error</response>  
        [HttpGet]
        [ProducesResponseType(typeof(APIInformation), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            try
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;

                return Ok(new APIInformation
                {
                    Version = version.ToString()
                });
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
