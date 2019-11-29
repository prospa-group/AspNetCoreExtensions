using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Sandbox.Api;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Builder
    // ReSharper restore CheckNamespace
{
    public static class StartupSecurityHeaders
    {
        public static IApplicationBuilder UseDefaultSecurityHeaders(this IApplicationBuilder app, IWebHostEnvironment hostingEnvironment)
        {
            if (!hostingEnvironment.IsDevelopment())
            {
                app.UseHsts(options => options.MaxAge(days: Constants.HttpHeaders.HstsMaxAgeDays).IncludeSubdomains().Preload().AllResponses());
                app.UseXfo(options => options.SameOrigin());
            }

            return app;
        }
    }
}