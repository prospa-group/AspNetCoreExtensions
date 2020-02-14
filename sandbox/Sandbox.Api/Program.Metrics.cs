using System;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Extensions.Configuration;
using App.Metrics.Reporting.GrafanaCloudHostedMetrics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Sandbox.Api
{
    public static class ProgramMetrics
    {
        public static IHostBuilder UseDefaultMetrics(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseMetrics();

            if (!Constants.Environments.IsDevelopment())
            {
                hostBuilder.ConfigureServices(services => services.AddApplicationInsightsTelemetry());
            }

            return hostBuilder;
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

        public static IHostBuilder ConfigureDefaultMetrics(this IHostBuilder webHostBuilder, IMetricsRoot metrics)
        {
            webHostBuilder.ConfigureMetrics(metrics);

            return webHostBuilder;
        }
    }
}