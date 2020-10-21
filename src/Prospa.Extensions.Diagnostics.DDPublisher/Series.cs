using System.Text.Json.Serialization;

namespace Prospa.Extensions.Diagnostics.DDPublisher
{
    public class Series
    {
        public string Metric { get; set; }

        public string Type { get; set; }

        public long Interval { get; set; }

        public string Host { get; set; }

        public string[] Tags { get; set; }

        public object[][] Points { get; set; }

        [JsonPropertyName("host_name")]
        public string HostName { get; set; }
    }
}