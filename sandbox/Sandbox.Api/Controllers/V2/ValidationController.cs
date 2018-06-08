using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Sandbox.Api.Application.Commands;
using Sandbox.Api.Routing;

namespace Sandbox.Api.Controllers.V2
{
    [VersionedRoute("[controller]"), V2]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        [HttpGet("default")]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public ActionResult<string> Default() { return ValidationProblem(); }

        [HttpGet("throw")]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public ActionResult<string> Throw() { throw new ValidationException("test validation exception"); }

        [HttpGet("loan/{id}")]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public ActionResult GetLoan(string id)
        {
            return Ok(new { Id = id });
        }

        [HttpPost("loan/fluent")]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public ActionResult PostLoanFluent(RequestFluentLoanApprovalCommand command)
        {
            return CreatedAtAction(nameof(GetLoan), new { id = command.Id }, new { Id = command.Id });
        }

        [HttpPost("loan")]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public ActionResult PostLoan(RequestLoanApprovalCommand command)
        {
            return CreatedAtAction(nameof(GetLoan), new { id = command.Id }, new { Id = command.Id });
        }
    }
}