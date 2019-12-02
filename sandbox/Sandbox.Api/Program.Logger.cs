using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Sandbox.Api
{
    public static class ProgramLogger
    {
        public static ILogger CreateDefaultLogger(
            this IHost host,
            string environment)
        {
            var configuration = host.Services.GetRequiredService<IConfiguration>();
            var loggerConfiguration = new LoggerConfiguration()
                                      .ReadFrom
                                      .Configuration(configuration)
                                      .Enrich
                                      .WithDefaults();

            var seqServerUrl = configuration.GetValue<string>(Constants.ConfigurationKeys.Seq.SeqServerUrl);

            if (!string.IsNullOrWhiteSpace(seqServerUrl))
            {
                loggerConfiguration.WriteTo.Seq(seqServerUrl);
            }

            if (environment == Constants.Environments.Development)
            {
                loggerConfiguration.WriteTo.Console(theme: AnsiConsoleTheme.Literate);
            }
            else
            {
                loggerConfiguration
                    .WriteTo
                    .ApplicationInsights(TelemetryConverter.Traces, restrictedToMinimumLevel: LogEventLevel.Error);
            }

            return loggerConfiguration.CreateLogger();
        }
    }
}