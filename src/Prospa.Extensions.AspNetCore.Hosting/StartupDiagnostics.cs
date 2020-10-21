using System;
using GlobalExceptionHandler.WebApi;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Prospa.Extensions.ApplicationInsights;
using Prospa.Extensions.AspNetCore.Http;
using Prospa.Extensions.AspNetCore.Http.Builder;
using Prospa.Extensions.AspNetCore.Http.Middlewares;
using Prospa.Extensions.AspNetCore.Mvc.Core.StartupFilters;
using Prospa.Extensions.AspNetCore.Serilog;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class StartupDiagnostics
    {
        public static IServiceCollection AddProspaDefaultDiagnostics(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<HttpErrorLogOptions>(configuration.GetSection(nameof(HttpErrorLogOptions)));
            services.AddSingleton(provider => provider.GetRequiredService<IOptions<HttpErrorLogOptions>>().Value);
            services.AddScoped<IHttpRequestDetailsLogger, HttpRequestDetailsSerilogLogger>();

            services.AddApplicationInsightsTelemetry();
            services.AddSingleton<ITelemetryInitializer, ActivityTagTelemetryInitializer>();
            services.AddApplicationInsightsTelemetryProcessor<AzureDependencyFilterTelemetryProcessor>();

            return services;
        }

        public static IServiceCollection AddProspaMetaEndpointProtection(
            this IServiceCollection services,
            HostBuilderContext context,
            params string[] endpoints)
        {
            services.AddSingleton<IStartupFilter>(new RequireEndpointKeyStartupFilter(endpoints, context.Configuration.GetValue<string>("EndpointKey")));

            return services;
        }

        public static IApplicationBuilder UseProspaDefaultDiagnostics(
            this IApplicationBuilder app,
            IWebHostEnvironment hostingEnvironment,
            Action<DiagnosticActivityMiddlewareOptions> optionsSetup)
        {
            var options = new DiagnosticActivityMiddlewareOptions();
            optionsSetup(options);

            app.UseDiagnosticActivityTagging(options);

            app.UseGlobalExceptionHandler(
                configuration =>
                {
                    configuration.HandleHttpValidationExceptions(hostingEnvironment);
                    configuration.HandleOperationCancelledExceptions(hostingEnvironment);
                    configuration.HandleUnauthorizedExceptions(hostingEnvironment);
                    configuration.HandleUnhandledExceptions(hostingEnvironment);
                });

            return app;
        }

        public static IApplicationBuilder UseProspaDefaultDiagnostics(this IApplicationBuilder app, IWebHostEnvironment hostingEnvironment)
        {
            app.UseProspaDefaultDiagnostics(
                hostingEnvironment,
                options =>
                {
                    options.HeadersToTag = new[] { "X-Original-For" };
                });

            return app;
        }
    }
}
