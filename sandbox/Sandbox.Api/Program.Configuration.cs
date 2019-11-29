using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Sandbox.Api
{
    public static class ProgramConfiguration
    {
        public static IHostBuilder ConfigureDefaultAppConfiguration(this IHostBuilder webHostBuilder, string[] args)
        {
            webHostBuilder.ConfigureAppConfiguration(
                (context, config) =>
                {
                    config.AddDefaultSources(args);
                });

            return webHostBuilder;
        }

        public static IConfigurationBuilder AddDefaultSources(this IConfigurationBuilder builder, string[] args = null)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .AddJsonFile($"appsettings.{Constants.Environments.CurrentAspNetCoreEnv ?? Constants.Environments.Production}.json", optional: true)
                   .AddEnvironmentVariables();

            if (Constants.Environments.IsDevelopment())
            {
                // config.AddUserSecrets(Assembly.GetExecutingAssembly());
            }

            if (args != null)
            {
                builder.AddCommandLine(args);
            }

            return builder;
        }
    }
}