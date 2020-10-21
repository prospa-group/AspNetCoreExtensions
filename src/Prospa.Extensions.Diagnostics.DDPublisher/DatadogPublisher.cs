using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Prospa.Extensions.Diagnostics.DDPublisher
{
    public class DatadogPublisher : IHealthCheckPublisher
    {
        private const string MetricNameTag = "metric_name";
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

                var sanitizedKey = keyedEntry.Key.Replace(' ', '_');
                var entry = keyedEntry.Value;
                var metricNameTag = string.Empty;
                var metricName = string.Empty;
                var sanitizedTags = new List<string>();

                foreach (var tag in entry.Tags)
                {
                    var sanitizedTag = tag.Replace(' ', '_');

                    var tagKeyValue = tag.Split(':');

                    if (tagKeyValue.Length == 0)
                    {
                        sanitizedTags.Add(sanitizedTag);
                        continue;
                    }

                    if (tagKeyValue[0] == MetricNameTag)
                    {
                        metricNameTag = tag;
                        metricName = !string.IsNullOrWhiteSpace(_configuration.MetricNamePrefix)
                            ? $"{_configuration.MetricNamePrefix}.{tagKeyValue[1].Replace(' ', '_')}"
                            : tagKeyValue[1].Replace(' ', '_');
                        continue;
                    }

                    sanitizedTags.Add(sanitizedTag);
                }

                if (string.IsNullOrWhiteSpace(metricNameTag))
                {
                    throw new ArgumentException($"metric_name tag is required on health check {keyedEntry.Value}");
                }

                var dataDogStatus = entry.Status switch
                {
                    HealthStatus.Healthy => Status.Ok,
                    HealthStatus.Degraded => Status.Warning,
                    HealthStatus.Unhealthy => Status.Critical,
                    _ => Status.Unknown
                };

                var p3domainTag = $"p3domain:{_configuration.Domain}".ToLower();
                var p3appTag = $"p3app:{_configuration.Application}".ToLower();
                var p3envTag = $"p3env:{_configuration.Environment}".ToLower();
                var healthCheckTag = $"{_configuration.ServiceTagPrefix}:{sanitizedKey}".ToLower();

                var tags = _configuration
                           .DefaultTags
                           .Concat(sanitizedTags)
                           .Concat(new[] { p3domainTag, p3appTag, p3envTag, healthCheckTag }).ToArray();

                metric.AddMetric(metricName, (int)dataDogStatus, (long)entry.Duration.TotalMilliseconds, tags);
            }

            await _client.SendMetricsAsync(metric);
        }
    }
}
