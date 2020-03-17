#if NETCOREAPP
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Prospa.Extensions.AspNetCore.Http.Middlewares
{
    public class DiagnosticActivityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly DiagnosticActivityMiddlewareOptions _options;

        public DiagnosticActivityMiddleware(RequestDelegate next, DiagnosticActivityMiddlewareOptions options)
        {
            _next = next;
            _options = options;
        }

        public Task Invoke(HttpContext httpContext)
        {
            if (httpContext.User?.FindFirst("sub")?.Value is string sub)
            {
                Activity.Current?.AddTag("UserId", sub);
            }

            foreach (var item in _options.RouteValuesToTag)
            {
                if (httpContext.Request.RouteValues[item] is string routeValue)
                {
                    Activity.Current?.AddTag(item, routeValue);
                }
            }

            foreach (var item in _options.QueryStringValuesToTag)
            {
                if (httpContext.Request.Query[item] is StringValues queryStringValues)
                {
                    Activity.Current?.AddTag(item, queryStringValues);
                }
            }

            foreach (var item in _options.HeadersToTag)
            {
                if (httpContext.Request.Headers.TryGetValue(item, out StringValues headerValue))
                {
                    Activity.Current?.AddTag(item, headerValue);
                }
            }

            if (!_options.DisableAddingCorrelationIdAsActivityBaggage)
            {
                if (httpContext.Request.Headers.TryGetValue("x-correlation-id", out StringValues correlationId))
                {
                    Activity.Current?.AddBaggage("CorrelationId", correlationId);
                }
            }

            if (!_options.DisableClientIdActvityTagging)
            {
                if (httpContext.User?.FindFirst("client_id") is Claim clientIdClaim)
                {
                    Activity.Current?.AddTag("ClientId", clientIdClaim.Value);
                }
            }

            return _next(httpContext);
        }
    }
}
#endif