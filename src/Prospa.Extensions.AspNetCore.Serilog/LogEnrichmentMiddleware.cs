using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Prospa.Extensions.AspNetCore.Serilog
{
    public class LogEnrichmentMiddleware
    {
        private readonly RequestDelegate _next;

        public LogEnrichmentMiddleware(RequestDelegate next) { _next = next; }

        public async Task Invoke(HttpContext context)
        {
            using (LogContext.Push(new HttpContextEnricher(context)))
            {
                await _next(context);
            }
        }
    }
}