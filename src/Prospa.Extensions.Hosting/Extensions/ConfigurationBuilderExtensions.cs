using System;
using Azure.Core;
using Azure.Identity;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Prospa.Extensions.Hosting;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.Configuration
    // ReSharper restore CheckNamespace
{
    public static class ConfigurationBuilderExtensions
    {
        public static void AddSharedAppConfiguration(this IConfigurationBuilder builder)
        {
            var config = builder.Build();
            var appConfigEndpoint = config.GetValue<string>(ProspaConstants.SharedConfigurationKeys.SharedAzureAppConfigurationEndpoint);

            if (string.IsNullOrWhiteSpace(appConfigEndpoint))
            {
                throw new Exception($"Missing App Configuration, Key: {ProspaConstants.SharedConfigurationKeys.SharedAzureAppConfigurationEndpoint}");
            }

            var credentials = ProspaConstants.Environments.IsDevelopment
                ? new DefaultAzureCredential()
                : (TokenCredential)new ManagedIdentityCredential();

            builder.AddAzureAppConfiguration(
                options =>
                {
                    options.Connect(new Uri(appConfigEndpoint), credentials);
                    options.ConfigureKeyVault(kv => kv.SetCredential(credentials));
                });
        }

        public static string SharedAzureServiceBusConnection(this IConfiguration configuration)
        {
            var connection = configuration.GetValue<string>(ProspaConstants.SharedConfigurationKeys.AzureServiceBusConnection);

            if (string.IsNullOrWhiteSpace(connection))
            {
                throw new Exception($"Missing App Configuration, Key: {ProspaConstants.SharedConfigurationKeys.AzureServiceBusConnection}");
            }

            return connection;
        }
    }
}