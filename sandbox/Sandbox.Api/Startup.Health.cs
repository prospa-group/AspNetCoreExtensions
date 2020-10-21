using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prospa.Extensions.Diagnostics.DDPublisher;
using Prospa.Extensions.Hosting;
using Sandbox.Api.Application.HealthChecks;

namespace Sandbox.Api
{
    public static class StartupHealth
    {
        public static IServiceCollection AddDefaultHealth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                    .AddCheck<AzureStorageAccountContainerOneConnectivityHealthCheck>(
                         "Container One Health Check",
                         tags: new[]
                               {
                                   DefaultHealthMetrics.AzureStorageContainerConnectivity,
                                   DefaultHealthMetrics.AzureResourceName("storage_account_one"),
                                   DefaultHealthMetrics.AzureResourceName("container_one")
                               })
                    .AddCheck<AzureStorageAccountContainerTwoConnectivityHealthCheck>(
                         "Container Two Health Check",
                         tags: new[]
                               {
                                   DefaultHealthMetrics.AzureStorageContainerConnectivity,
                                   DefaultHealthMetrics.AzureResourceName("storage_account_one"),
                                   DefaultHealthMetrics.AzureResourceName("container_two")
                               }).AddDatadogPublisher(
                         cfg =>
                         {
                             cfg.ServiceCheckName = typeof(Program).Assembly.GetName().Name.ToLower();
                             cfg.Domain = configuration.GetValue<string>("AppDomain");
                             cfg.Application = typeof(Program).Assembly.GetName().Name;
                             cfg.Environment = ProspaConstants.Environments.CurrentEnv;
                             cfg.ApiKey = "ApiKey";
                             cfg.ApplicationKey = "ApplicationKey";
                             cfg.Url = "https://api.datadoghq.com/api";
                         });

            return services;
        }

        public static IApplicationBuilder UseDefaultHealth(this IApplicationBuilder builder)
        {
            builder.UseHealthChecks(
                Constants.HealthEndpoint,
                new HealthCheckOptions
                {
                    ResponseWriter = ProspaConstants.WriteHealthResponse
                });

            return builder;
        }
    }
}