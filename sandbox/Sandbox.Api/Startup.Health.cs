using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Prospa.Extensions.Diagnostics.DDPublisher;
using Sandbox.Api.Application.HealthChecks;

namespace Sandbox.Api
{
    public static class StartupHealth
    {
        public static IApplicationBuilder UseDefaultHealth(this IApplicationBuilder builder)
        {
            builder.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            return builder;
        }

        public static IServiceCollection AddDefaultHealth(this IServiceCollection services)
        {
            services.AddHealthChecks()
                    .AddCheck<SampleHealthCheck>("sample_health_check")
                    .AddDatadogPublisher(
                        configuration =>
                        {
                            configuration.ServiceCheckName = "sandbox.api";
                            configuration.ApiKey = "ApiKey";
                            configuration.ApplicationKey = "ApplicationKey";
                            configuration.Url = "https://api.datadoghq.com/api";
                        });

            services.AddHealthChecksUI();

            return services;
        }
    }
}
