using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;

namespace Prospa.Extensions.AspNetCore.ApplicationInsights.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDefaultApplicationInsightsExtensions(this IServiceCollection services)
        {
            services.AddSingleton<ITelemetryInitializer, ActivityTagTelemetryInitializer>();
            services.AddApplicationInsightsTelemetryProcessor<AzureDependencyTelemetryProcessor>();
            return services;
        }
    }
}