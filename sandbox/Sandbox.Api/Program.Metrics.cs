using System;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Extensions.Configuration;
using App.Metrics.Reporting.GrafanaCloudHostedMetrics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prospa.Extensions.AspNetCore.Mvc.Core.StartupFilters;

namespace Sandbox.Api
{
    public static class ProgramMetrics
    {
        public static IWebHostBuilder UseDefaultMetrics(this IWebHostBuilder webHostBuilder)
        {
            webHostBuilder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IStartupFilter>(new RequireEndpointKeyStartupFilter(new[] { "/health", "/metrics", "/metrics-text", "/env", "/docs" }, "123123"));
            });

            webHostBuilder.UseMetrics();

            if (!Constants.Environments.IsDevelopment())
            {
                webHostBuilder.UseApplicationInsights();
            }

            return webHostBuilder;
        }

        public static IMetricsRoot BuildDefaultMetrics(this IMetricsBuilder builder)
        {
            var configuration = new ConfigurationBuilder().AddDefaultSources().Build();

            // Samples with weight of less than 10% of average should be discarded when rescaling
            const double minimumSampleWeight = 0.001;

            builder.Configuration.ReadFrom(configuration);

            builder.SampleWith.ForwardDecaying(
                AppMetricsReservoirSamplingConstants.DefaultSampleSize,
                AppMetricsReservoirSamplingConstants.DefaultExponentialDecayFactor,
                minimumSampleWeight: minimumSampleWeight);

            if (!Constants.Environments.IsDevelopment())
            {
                var grafanaCloudHostedMetricsOptions = new MetricsReportingHostedMetricsOptions();
                configuration.GetSection(nameof(MetricsReportingHostedMetricsOptions)).Bind(grafanaCloudHostedMetricsOptions);

                if (string.IsNullOrWhiteSpace(grafanaCloudHostedMetricsOptions.HostedMetrics.ApiKey))
                {
                    throw new ApplicationException("Hosted Metrics ApiKey Missing, add MetricsReportingHostedMetricsOptions--HostedMetrics--ApiKey to KeyVault");
                }

                builder.Report.ToHostedMetrics(grafanaCloudHostedMetricsOptions);
            }

            return builder.Build();
        }

        public static IWebHostBuilder ConfigureDefaultMetrics(this IWebHostBuilder webHostBuilder, IMetricsRoot metrics)
        {
            webHostBuilder.ConfigureMetrics(metrics);

            return webHostBuilder;
        }
    }
}