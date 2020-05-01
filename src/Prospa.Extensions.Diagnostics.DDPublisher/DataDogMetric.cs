using System;
using System.Collections.Generic;
using System.Linq;

namespace Prospa.Extensions.Diagnostics.DDPublisher
{
    public class DataDogMetric
    {
        private readonly string _host;

        public DataDogMetric()
            : this(string.Empty)
        {
        }

        public DataDogMetric(string host)
        {
            _host = host;
            Series = new List<Series>();
        }

        public List<Series> Series { get; }

        public void AddMetric(string metricName, int metricValue, long metricInterval, IEnumerable<string> metricTags)
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
                                Host = _host,
                                HostName = Environment.MachineName
                            };

            Series.Add(series);
        }
    }
}