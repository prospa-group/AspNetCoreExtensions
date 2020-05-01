using System;
using System.ComponentModel.DataAnnotations;

namespace Prospa.Extensions.Diagnostics.DDPublisher
{
    public class DatadogConfiguration
    {
        [Required]
        public string Url { get; set; }

        [Required]
        public string ApiKey { get; set; }

        [Required]
        public string ApplicationKey { get; set; }

        public string[] DefaultTags { get; set; } = Array.Empty<string>();

        [Required]
        public string ServiceCheckName { get; set; }
    }
}