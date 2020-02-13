using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Sandbox.Api.Controllers
{
    [Route("health")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [ApiVersionNeutral]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckOptions _healthCheckOptions;
        private readonly HealthCheckService _healthCheckService;

        public HealthController(HealthCheckService healthCheckService, IOptions<HealthCheckOptions> healthCheckOptions)
        {
            _healthCheckService = healthCheckService;
            _healthCheckOptions = healthCheckOptions.Value;
        }

        [HttpGet]
        public async Task Get()
        {
            var result = await _healthCheckService.CheckHealthAsync(_healthCheckOptions.Predicate, HttpContext.RequestAborted);

            // Map status to response code - this is customizable via options.
            if (!_healthCheckOptions.ResultStatusCodes.TryGetValue(result.Status, out var statusCode))
            {
                var message =
                    $"No status code mapping found for {nameof(HealthStatus)} value: {result.Status}." +
                    $"{nameof(HealthCheckOptions)}.{nameof(HealthCheckOptions.ResultStatusCodes)} must contain" +
                    $"an entry for {result.Status}.";

                throw new InvalidOperationException(message);
            }

            HttpContext.Response.StatusCode = statusCode;

            if (!_healthCheckOptions.AllowCachingResponses)
            {
                var headers = HttpContext.Response.Headers;
                headers[HeaderNames.CacheControl] = "no-store, no-cache";
                headers[HeaderNames.Pragma] = "no-cache";
                headers[HeaderNames.Expires] = "Thu, 01 Jan 1970 00:00:00 GMT";
            }

            if (_healthCheckOptions.ResponseWriter != null)
            {
                await _healthCheckOptions.ResponseWriter(HttpContext, result);
            }
        }
    }
}
