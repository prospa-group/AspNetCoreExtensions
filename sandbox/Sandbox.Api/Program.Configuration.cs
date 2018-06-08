using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Sandbox.Api
{
    public static class ProgramConfiguration
    {
        public static IWebHostBuilder ConfigureDefaultAppConfiguration(this IWebHostBuilder webHostBuilder, string[] args)
        {
            webHostBuilder.ConfigureAppConfiguration(
                (context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{Constants.Environments.CurrentAspNetCoreEnv ?? Constants.Environments.Production}.json", optional: true)
                          .AddEnvironmentVariables();

                    if (Constants.Environments.IsDevelopment())
                    {
                        config.AddUserSecrets(Assembly.GetExecutingAssembly());
                    }

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                });

            return webHostBuilder;
        }
    }
}