using CorrelationId;
using Microsoft.AspNetCore.Mvc;
using Sandbox.Api.Routing;
using Serilog;

namespace Sandbox.Api.Controllers.V1
{
    [VersionedRoute("[controller]"), V1(Deprecated = true)]
    [ApiController]
    public class CorrelationController : ControllerBase
    {
        private readonly ICorrelationContextAccessor _correlationContext;
        private readonly ILogger _logger = Log.ForContext<CorrelationController>();

        public CorrelationController(ICorrelationContextAccessor correlationContext) { _correlationContext = correlationContext; }

        [HttpGet]
        public ActionResult<string> Get()
        {
            _logger.Information("logger test v1");

            return _correlationContext.CorrelationContext.CorrelationId;
        }
    }
}