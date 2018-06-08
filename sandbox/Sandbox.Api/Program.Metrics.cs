using App.Metrics.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Sandbox.Api.StartupFilters;

namespace Sandbox.Api
{
    public static class ProgramMetrics
    {
        public static IWebHostBuilder UseDefaultMetrics(this IWebHostBuilder webHostBuilder)
        {
            webHostBuilder.ConfigureServices((context, services) => services.AddSingleton<IStartupFilter>(new RequireKeyForMetricsAndHealthStartupFilter()));

            webHostBuilder.UseMetrics();

            if (!Constants.Environments.IsDevelopment())
            {
                webHostBuilder.UseApplicationInsights();
            }

            return webHostBuilder;
        }
    }
}