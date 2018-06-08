using GlobalExceptionHandler.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Prospa.Extensions.AspNetCore.Http;
using Prospa.Extensions.AspNetCore.Serilog;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class StartupDiagnostics
    {
        public static IServiceCollection AddDefaultDiagnostics(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<HttpErrorLogOptions>(configuration.GetSection(nameof(HttpErrorLogOptions)));
            services.AddSingleton(provider => provider.GetRequiredService<IOptions<HttpErrorLogOptions>>().Value);
            services.AddScoped<IHttpRequestDetailsLogger, HttpRequestDetailsSerilogLogger>();

            return services;
        }

        public static IApplicationBuilder UseDefaultDiagnostics(this IApplicationBuilder app, IHostingEnvironment hostingEnvironment)
        {
            app.UseMiddleware<LogEnrichmentMiddleware>();

            app.UseExceptionHandler("/error").WithConventions(x =>
            {
                x.HandleHttpValidationExceptions(hostingEnvironment);
                x.HandleOperationCancelledExceptions(hostingEnvironment);
                x.HandleUnauthorizedExceptions(hostingEnvironment);
                x.HandleUnhandledExceptions(hostingEnvironment);
            });

            return app;
        }
    }
}
