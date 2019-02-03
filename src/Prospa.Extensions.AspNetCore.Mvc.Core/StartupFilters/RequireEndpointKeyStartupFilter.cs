using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Prospa.Extensions.AspNetCore.Mvc.Core.StartupFilters
{
    /// <summary>
    /// Startup Filter to protect diagnostics endpoints such as health
    /// </summary>
    public class RequireEndpointKeyStartupFilter : IStartupFilter
    {
        private static readonly string[] DefaultEndpoints = { "/health", "/metrics", "/metrics-text", "/env", "/docs" };
        private readonly string[] _endpoints;
        private readonly string _key;

        public RequireEndpointKeyStartupFilter(string[] endpoints, string key)
        {
            _endpoints = endpoints;
            _key = key;
        }

        public RequireEndpointKeyStartupFilter(string key)
        {
            _endpoints = DefaultEndpoints;
            _key = key;
        }

        /// <inheritdoc />
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return RequireSecretToMetricsAndHealth;

            void RequireSecretToMetricsAndHealth(IApplicationBuilder app)
            {
                app.Use(async (context, next2) =>
                {
                    var key = ExtractToken(context);

                    if (DefaultEndpoints.Any(e => context.Request.Path.Value == e))
                    {
                        if (key != _key)
                        {
                            context.Abort();
                            return;
                        }
                    }

                    await next2.Invoke();
                });

                next(app);
            }
        }

        private static string ExtractToken(HttpContext context)
        {
            return context.Request.QueryString.HasValue && context.Request.Query.ContainsKey("EndpointKey")
                ? context.Request.Query["EndpointKey"]
                : StringValues.Empty;
        }
    }
}
