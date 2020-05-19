using System;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.Configuration
    // ReSharper restore CheckNamespace
{
    public static class ConfigurationExtensions
    {
        public static string GetNServiceBusLicense(this IConfiguration configuration)
        {
            var license = configuration.GetValue<string>("NServiceBus/License");

            if (string.IsNullOrWhiteSpace(license))
            {
                throw new Exception("Missing NServiceBus license. Configuration Key: 'NServiceBus/License'");
            }

            return license;
        }
    }
}