using System;
using System.Collections.Generic;
using System.Linq;

namespace Prospa.Extensions.Diagnostics.DDPublisher
{
    public class DataDogMetric
    {
        public DataDogMetric()
        {
            Series = new List<Series>();
        }

        private List<Series> Series { get; }

        public void AddMetric(string host, string metricName, int metricValue, long metricInterval, IEnumerable<string> metricTags)
        {
            var date = DateTime.UtcNow;
            var unixDate = ((DateTimeOffset)date).ToUnixTimeSeconds();

            var series = new Series
                            {
                                Metric = metricName,
                                Type = "gauge",
                                Points = new[]
                                         {
                                             new object[]
                                             {
                                                 unixDate,
                                                 metricValue
                                             }
                                         },
                                Interval = metricInterval,
                                Tags = metricTags.ToArray(),
                                Host = host,
                                HostName = Environment.MachineName
                            };

            Series.Add(series);
        }
    }
}