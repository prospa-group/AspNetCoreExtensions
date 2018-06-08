using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Sandbox.Api.StartupFilters
{
    public class RequireKeyForMetricsAndHealthStartupFilter : IStartupFilter
    {
        private static readonly string[] Endpoints = { "/health", "/metrics", "/metrics-text", "/env" };

        /// <inheritdoc />
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return RequireSecretToMetricsAndHealth;

            void RequireSecretToMetricsAndHealth(IApplicationBuilder app)
            {
                app.Use(async (context, next2) =>
                {
                    var key = ExtractKey(context);

                    if (Endpoints.Any(e => context.Request.Path.Value == e && StringValues.IsNullOrEmpty(key)))
                    {
                        context.Abort();
                        return;
                    }

                    await next2.Invoke();
                });

                next(app);
            }
        }

        private static string ExtractKey(HttpContext context)
        {
            return context.Request.QueryString.HasValue && context.Request.Query.ContainsKey(nameof(MetricsRegistry.EndpointKey))
                ? context.Request.Query[nameof(MetricsRegistry.EndpointKey)]
                : StringValues.Empty;
        }
    }
}
