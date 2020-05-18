using Microsoft.AspNetCore.Mvc.Versioning;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class StartupVersioning
    {
        public static IServiceCollection AddProspaDefaultApiVersioning(this IServiceCollection services)
        {
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