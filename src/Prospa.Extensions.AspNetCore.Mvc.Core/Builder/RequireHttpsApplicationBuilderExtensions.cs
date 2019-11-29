using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prospa.Extensions.AspNetCore.Mvc.Core.Resources;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Builder
    // ReSharper restore CheckNamespace
{
    public static class RequireHttpsApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRequireHttps(this IApplicationBuilder app, params string[] allowedPaths)
        {
            app.Use(
                (context, next) =>
                {
                    if (IsAllowedHttpPath(context, allowedPaths))
                    {
                        return next();
                    }

                    if (HasForwardProtoAndNotHttps(context))
                    {
                        return RequireHttpResponse(context);
                    }

                    if (!context.Request.IsHttps)
                    {
                        return RequireHttpResponse(context);
                    }

                    return next();
                });

            return app;
        }

        private static Task RequireHttpResponse(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/problem+json";

            var response = new ValidationProblemDetails
                          {
                              Instance = context.Request.Path,
                              Status = StatusCodes.Status403Forbidden,
                              Title = ErrorMessages.RequireHttpsTitle
                          };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static bool HasForwardProtoAndNotHttps(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("X-Forwarded-Proto", out var xproto))
            {
                return !xproto.ToString().StartsWith("https", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        private static bool IsAllowedHttpPath(HttpContext context, string[] allowedpaths)
        {
            return context.Request.Path.Value.EndsWith("/ping") || allowedpaths.Any(path => context.Request.Path.Value.EndsWith(path));
        }
    }
}