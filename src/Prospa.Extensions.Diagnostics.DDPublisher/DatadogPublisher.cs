using System;
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
            foreach (var keyedEntry in report.Entries)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                var key = keyedEntry.Key;
                var entry = keyedEntry.Value;

                var dataDogStatus = entry.Status switch
                {
                    HealthStatus.Healthy => Status.Ok,
                    HealthStatus.Degraded => Status.Warning,
                    HealthStatus.Unhealthy => Status.Critical,
                    _ => Status.Unknown
                };

                var tags = _configuration.DefaultTags.Concat(new[]
                                                             {
                                                                 $"check:{key}"
                                                             }).ToArray();

                var message = entry.Description ?? entry.Status.ToString();

                var metric = new DataDogMetric();

                metric.AddMetric(_configuration.ServiceCheckName, message, (int)dataDogStatus, (long)entry.Duration.TotalMilliseconds, tags);

                await _client.SendMetricsAsync(metric);
            }
        }
    }
}
