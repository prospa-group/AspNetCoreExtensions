using System;
using Destructurama;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Prospa.Extensions.Hosting;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.Hosting
    // ReSharper restore CheckNamespace
{
    public static class ProgramLogger
    {
        public static void CreateProspaDefaultLogger(
            this HostBuilderContext context,
            LoggerConfiguration loggerConfiguration,
            Type type)
        {
            _ = loggerConfiguration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.WithDefaults()
                .Enrich.WithDemystifiedStackTraces()
                .Destructure.UsingAttributes();

            if (ProspaConstants.Environments.IsDevelopment)
            {
                loggerConfiguration.WriteTo.Console(theme: AnsiConsoleTheme.Literate);
            }
            else
            {
                loggerConfiguration
                    .WriteTo
                    .ApplicationInsights(TelemetryConverter.Traces, restrictedToMinimumLevel: LogEventLevel.Error);
            }

            WriteToDataDog(context.Configuration, loggerConfiguration, type);

            _ = loggerConfiguration
                .WriteTo
                .ApplicationInsights(TelemetryConfiguration.CreateDefault(), TelemetryConverter.Events, LogEventLevel.Warning);
        }

        private static void WriteToDataDog(
            IConfiguration configuration,
            LoggerConfiguration loggerConfiguration,
            Type type)
        {
            var dataDogApiKey = configuration.GetValue<string>("DataDogApiKey");
            var appDomain = configuration.GetValue<string>("AppDomain");

            if (!string.IsNullOrWhiteSpace(dataDogApiKey))
            {
                loggerConfiguration.WriteTo.DatadogLogs(
                    dataDogApiKey,
                    source: appDomain,
                    service: type.Assembly.GetName().Name,
                    tags: new[] { $"env:{ProspaConstants.Environments.CurrentEnv}", $"p3domain:{appDomain}", $"p3app:{type.Assembly.GetName().Name}" },
                    logLevel: LogEventLevel.Information);
            }
        }
    }
}