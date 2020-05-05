using System;
using System.Reflection;
using CorrelationId;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Prospa.Extensions.AspNetCore.Hosting;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class StartupCore
    {
        public static IServiceCollection AddDefaultCoreServices(this IServiceCollection services, IConfiguration configuration, Type type)
        {
            services.AddCorrelationId();

            services
                    .AddMvcCore()
                    .AddDefaultValidation(type)
                    .AddApiExplorer()
                    .AddAuthorization()
                    .AddDataAnnotations();

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddApiVersioning()
                    .AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'V");

            services
                .AddRouting(options => options.LowercaseUrls = true)
                .AddDefaultApiVersioning()
                .AddDefaultDiagnostics(configuration)
                .AddDefaultAuthenticationAndAuthorization(configuration)
                .AddDefaultSwagger(type.GetTypeInfo().Assembly);

            return services;
        }
    }
}