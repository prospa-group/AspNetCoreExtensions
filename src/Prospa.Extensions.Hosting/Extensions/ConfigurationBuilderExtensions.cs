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
        public static void AddSharedKeyvault(this IConfigurationBuilder builder)
        {
            var keyVaultEndpoint = $"https://{ProspaConstants.Environments.Prefix()}3p-shared-kv.vault.azure.net/";
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient =
                new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            builder.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
        }

        public static void AddSharedAppConfiguration(this IConfigurationBuilder builder)
        {
            var config = builder.Build();
            builder.AddAzureAppConfiguration(config.GetValue<string>(ProspaConstants.SharedConfigurationKeys.AzureAppConfiguration));
        }

        public static string SharedAzureServiceBusConnection(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(ProspaConstants.SharedConfigurationKeys.AzureServiceBusConnection);
        }
    }
}