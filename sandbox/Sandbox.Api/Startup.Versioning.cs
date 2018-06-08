using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class StartupVersioning
    {
        public static IServiceCollection AddDefaultApiVersioning(this IServiceCollection services)
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<MvcOptions>, ConventionsMvcOptionsSetup>());

            services.AddApiVersioning(
                options =>
                {
                    options.ApiVersionReader = new UrlSegmentApiVersionReader();
                    options.ReportApiVersions = true;
                });

            return services;
        }
    }
}