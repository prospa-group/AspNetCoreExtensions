using App.Metrics;
using App.Metrics.AspNetCore.Health;
using App.Metrics.Health;
using App.Metrics.Health.Reporting.Slack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Sandbox.Api
{
    public static class ProgramHealth
    {
        public static IWebHostBuilder UseDefaultHealth(this IWebHostBuilder webHostBuilder)
        {
            webHostBuilder.UseHealth();

            return webHostBuilder;
        }

        public static IWebHostBuilder ConfigureDefaultHealth(this IWebHostBuilder webHostBuilder, IMetrics metrics)
        {
            webHostBuilder.ConfigureHealthWithDefaults(
                (context, builder) =>
                {
                    var slackOptions = new SlackHealthAlertOptions();
                    context.Configuration.GetSection(nameof(SlackHealthAlertOptions)).Bind(slackOptions);

                    builder.Report.ToMetrics(metrics);
                    builder.Report.ToSlack(slackOptions);
                });

            return webHostBuilder;
        }
    }
}
