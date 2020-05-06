using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Prospa.Extensions.AspNetCore.Hosting;

namespace Sandbox.Api
{
    public static class ProgramConfiguration
    {
        public static void AddDefaultKeyvault(this IConfigurationBuilder builder)
        {
            var builtConfig = builder.Build();
            var keyVaultName = builtConfig.GetValue<string>("KeyVaultName");
            var keyVaultEndpoint = $"https://{ProspaConstants.Environments.Prefix()}{keyVaultName}.vault.azure.net/";
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            builder.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
        }
    }
}