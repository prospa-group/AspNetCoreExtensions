using CorrelationId;
using Microsoft.AspNetCore.Mvc;
using Prospa.Extensions.AspNetCore.Mvc.Core.Filters;
using Sandbox.Api.Routing;

namespace Sandbox.Api.Controllers.V2
{
    [VersionedRoute("[controller]"), V2]
    [ApiController]
    public class CorrelationController : ControllerBase
    {
        private readonly ICorrelationContextAccessor _correlationContext;

        public CorrelationController(ICorrelationContextAccessor correlationContext) { _correlationContext = correlationContext; }

        [HttpGet("optional")]
        [CorrelationIdHttpHeader(Required = false)]
        public ActionResult<string> GetOptional()
        {
            return _correlationContext.CorrelationContext.CorrelationId;
        }

        [HttpGet("require")]
        [CorrelationIdHttpHeader]
        public ActionResult<string> GetRequire()
        {
            return _correlationContext.CorrelationContext.CorrelationId;
        }

        [HttpGet("no-forward")]
        [CorrelationIdHttpHeader(Forward = false)]
        public ActionResult<string> GetForward()
        {
            return _correlationContext.CorrelationContext.CorrelationId;
        }
    }
}