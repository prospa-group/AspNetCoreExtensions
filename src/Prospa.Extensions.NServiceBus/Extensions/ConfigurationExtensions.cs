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

        public static string AzureServiceBusConnection(this IConfiguration configuration)
        {
            var connection = configuration.GetValue<string>("AzureServiceBusConnection");

            if (string.IsNullOrWhiteSpace(connection))
            {
                throw new Exception("Missing App Configuration, Key: 'AzureServiceBusConnection'");
            }

            return connection;
        }
    }
}