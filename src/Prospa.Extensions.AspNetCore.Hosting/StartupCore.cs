using System;
using System.Reflection;
using CorrelationId;
using CorrelationId.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Prospa.Extensions.AspNetCore.Hosting;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class StartupCore
    {
        public static IServiceCollection AddProspaDefaultServices(
            this IServiceCollection services,
            IConfiguration configuration,
            Type type)
        {
            services.AddDefaultCorrelationId();

            services.AddApiVersioning()
                    .AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'V");

            services
                .AddProspaDefaultApiVersioning()
                .AddProspaDefaultDiagnostics(configuration)
                .AddProspaDefaultAuthenticationAndAuthorization(configuration)
                .AddProspaDefaultSwagger(type.GetTypeInfo().Assembly);

            return services;
        }
    }
}