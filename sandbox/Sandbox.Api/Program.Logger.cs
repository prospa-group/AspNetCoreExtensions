using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Sandbox.Api
{
    public static class ProgramLogger
    {
        public static ILogger CreateDefaultLogger(
            this IWebHost webHost,
            string environment)
        {
            var configuration = webHost.Services.GetRequiredService<IConfiguration>();
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
                    .ApplicationInsightsTraces(
                        configuration.GetValue<string>(Constants.ConfigurationKeys.AppInsights.InstrumentationKey),
                        restrictedToMinimumLevel: LogEventLevel.Error);
            }

            return loggerConfiguration.CreateLogger();
        }
    }
}