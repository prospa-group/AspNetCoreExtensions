using System;
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
            var appConfigConnection = config.GetValue<string>(ProspaConstants.SharedConfigurationKeys.AzureAppConfiguration);

            if (string.IsNullOrWhiteSpace(appConfigConnection))
            {
                throw new Exception($"Missing App Configuration: {ProspaConstants.SharedConfigurationKeys.AzureAppConfiguration}");
            }

            builder.AddAzureAppConfiguration(appConfigConnection);
        }

        public static void AddSharedKeyvault(this IConfigurationBuilder builder, string keyvaultName)
        {
            var keyVaultEndpoint = $"https://{ProspaConstants.Environments.Prefix()}{keyvaultName}.vault.azure.net/";
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient =
                new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            builder.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
        }

        public static string SharedAzureServiceBusConnection(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(ProspaConstants.SharedConfigurationKeys.AzureServiceBusConnection);
        }
    }
}