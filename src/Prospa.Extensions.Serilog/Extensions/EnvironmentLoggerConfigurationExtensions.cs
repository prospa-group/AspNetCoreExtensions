using System;
using Serilog.Core;
using Serilog.Exceptions;

// ReSharper disable CheckNamespace
namespace Serilog.Configuration
    // ReSharper restore CheckNamespace
{
    public static class EnvironmentLoggerConfigurationExtensions
    {
        public static LoggerConfiguration WithApplicationName(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null)
            {
                throw new ArgumentNullException(nameof(enrichmentConfiguration));
            }

            return enrichmentConfiguration.With<ApplicationNameEnricher>();
        }

        public static LoggerConfiguration WithApplicationVersion(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null)
            {
                throw new ArgumentNullException(nameof(enrichmentConfiguration));
            }

            return enrichmentConfiguration.With<ApplicationVersionEnricher>();
        }

        public static LoggerConfiguration WithDefaults(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            return enrichmentConfiguration
                   .WithExceptionDetails()
                   .Enrich.WithApplicationName()
                   .Enrich.WithApplicationVersion()
                   .Enrich.WithEnvironmentName()
                   .Enrich.WithMachineName()
                   .Enrich.WithThreadId()
                   .Enrich.FromLogContext();
        }

        public static LoggerConfiguration WithEnvironmentName(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null)
            {
                throw new ArgumentNullException(nameof(enrichmentConfiguration));
            }

            return enrichmentConfiguration.With<EnvironmentNameEnricher>();
        }
    }
}