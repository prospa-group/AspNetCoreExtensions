using System;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Prospa.Extensions.Hosting;

namespace Sandbox.Api
{
    public static class ProgramConfiguration
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
                ? new InteractiveBrowserCredential()
                : (TokenCredential)new ManagedIdentityCredential();

            builder.AddAzureAppConfiguration(
                options =>
                {
                    options.Connect(new Uri(appConfigEndpoint), credentials);
                    options.ConfigureKeyVault(kv => kv.SetCredential(credentials));
                    options.Select("SHARED:*");
                    options.TrimKeyPrefix("SHARED:");
                });
        }
    }
}