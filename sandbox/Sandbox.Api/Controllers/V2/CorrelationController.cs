using CorrelationId;
using Microsoft.AspNetCore.Mvc;
using Prospa.Extensions.AspNetCore.Mvc.Core.Filters;
using Sandbox.Api.Routing;
using Serilog;

namespace Sandbox.Api.Controllers.V2
{
    [VersionedRoute("[controller]"), V2]
    [ApiController]
    public class CorrelationController : ControllerBase
    {
        private static readonly ILogger Logger = Log.ForContext<CorrelationController>();
        private readonly ICorrelationContextAccessor _correlationContext;

        public CorrelationController(ICorrelationContextAccessor correlationContext) { _correlationContext = correlationContext; }

        [HttpGet("optional")]
        [CorrelationIdHttpHeader(Required = false)]
        public ActionResult<string> GetOptional()
        {
            System.Diagnostics.Activity.Current?.AddTag("TestTag", "1");
            System.Diagnostics.Activity.Current?.AddBaggage("TestBaggage", "2");

            Logger.Information("Test to see if diagnostic tag is added");
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