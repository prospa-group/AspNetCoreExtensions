using Microsoft.AspNetCore.Builder;
using Prospa.Extensions.AspNetCore.Http.Middlewares;

namespace Prospa.Extensions.AspNetCore.Http.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDiagnosticActivityTagging(
            this IApplicationBuilder app,
            DiagnosticActivityMiddlewareOptions options = null)
        {
            return app.UseMiddleware<DiagnosticActivityMiddleware>(options ?? new DiagnosticActivityMiddlewareOptions());
        }
    }
}