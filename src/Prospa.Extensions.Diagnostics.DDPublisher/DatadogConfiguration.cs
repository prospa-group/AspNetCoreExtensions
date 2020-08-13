using System;
using System.ComponentModel.DataAnnotations;

namespace Prospa.Extensions.Diagnostics.DDPublisher
{
    public class DatadogConfiguration
    {
        [Required]
        public string ApiKey { get; set; }

        [Required]
        public string ApplicationKey { get; set; }

        [Required]
        public string Domain { get; set; }

        [Required]
        public string Application { get; set; }

        [Required]
        public string Environment { get; set; }

        public string[] DefaultTags { get; set; } = Array.Empty<string>();

        public string MetricNamePrefix { get; set; }

        [Required]
        public string ServiceCheckName { get; set; }

        public string ServiceTagPrefix { get; set; } = "check";

        [Required]
        public string Url { get; set; }
    }
}