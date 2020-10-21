using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Prospa.Extensions.AspNetCore.Mvc.Versioning.Swagger.Routing;

namespace Sandbox.Api.Controllers.V2
{
    [VersionedRoute("[controller]"), V2]
    [ApiController]
    public class ExceptionController : Controller
    {
        [HttpGet("throw-validation-exception")]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public ActionResult ThrowValidationException()
        {
            throw new ValidationException("testing validation exception");
        }

        [HttpGet("throw-application-exception")]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public ActionResult ThrowException()
        {
            throw new ApplicationException("testing application exception");
        }

        [HttpGet("throw-unauthorized-access-exception")]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        public IActionResult GetUnauth()
        {
            throw new UnauthorizedAccessException();
        }
    }
}
