using System;
using System.IO;
using App.Metrics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prospa.Extensions.AspNetCore.Mvc.Core.StartupFilters;
using Serilog;

namespace Sandbox.Api
{
    public sealed class Program
    {
        public static int Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();

            Log.Logger = webHost.CreateDefaultLogger(Constants.Environments.CurrentAspNetCoreEnv);

            try
            {
                webHost.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly, check the application's WebHost configuration.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var metrics = AppMetrics.CreateDefaultBuilder().BuildDefaultMetrics();

            return Host.CreateDefaultBuilder(args)
                   .ConfigureDefaultAppConfiguration(args)
                   .ConfigureServices((context, services) =>
                   {
                       services.AddSingleton<IStartupFilter>(
                           new RequireEndpointKeyStartupFilter(
                               new[] { "/health", "/metrics", "/metrics-text", "/env", "/docs" },
                           context.Configuration.GetValue<string>(Constants.Auth.EndpointKey)));
                   })
                   .ConfigureDefaultMetrics(metrics)
                   .UseSerilog()
                   .UseDefaultMetrics()
                   .ConfigureWebHostDefaults(webHostBuilder =>
                    {
                        webHostBuilder
                            .ConfigureKestrel(options => { options.AddServerHeader = false; })
                            .UseStartup<Startup>();
                    });
        }
    }
}