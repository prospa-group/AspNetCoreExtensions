using System;
using Prospa.Extensions.AspNetCore.Http.DelegatingHandlers;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpLoggingDelegatingHandler(this IServiceCollection services, Action<LoggingDelegatingHandlerOptions> configureLogging = null)
        {
            var options = new LoggingDelegatingHandlerOptions();
            configureLogging?.Invoke(options);
            services.AddSingleton(options);

            services.AddTransient<LoggingDelegatingHandler>();
            return services;
        }
    }
}
