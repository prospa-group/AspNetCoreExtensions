using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Prospa.Extensions.Diagnostics.DDPublisher
{
    public class DatadogPublisher : IHealthCheckPublisher
    {
        private readonly DatadogConfiguration _configuration;
        private readonly DatadogHttpClient _client;

        public DatadogPublisher(DatadogConfiguration configuration, DatadogHttpClient client)
        {
            _configuration = configuration;
            _client = client;
        }

        public async Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
        {
            var metric = new DataDogMetric(_configuration.ServiceCheckName);

            foreach (var keyedEntry in report.Entries)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                var key = keyedEntry.Key;
                var entry = keyedEntry.Value;
                var metricName = !string.IsNullOrWhiteSpace(_configuration.MetricNamePrefix)
                    ? $"{_configuration.MetricNamePrefix}.{key.Replace(' ', '_')}"
                    : key.Replace(' ', '_');

                var dataDogStatus = entry.Status switch
                {
                    HealthStatus.Healthy => Status.Ok,
                    HealthStatus.Degraded => Status.Warning,
                    HealthStatus.Unhealthy => Status.Critical,
                    _ => Status.Unknown
                };

                var tags = _configuration
                           .DefaultTags
                           .Concat(entry.Tags)
                           .Concat(new[]
                           {
                                $"{_configuration.ServiceTagPrefix}:{key}"
                           }).ToArray();

                metric.AddMetric(metricName, (int)dataDogStatus, (long)entry.Duration.TotalMilliseconds, tags);
            }

            await _client.SendMetricsAsync(metric);
        }
    }
}
