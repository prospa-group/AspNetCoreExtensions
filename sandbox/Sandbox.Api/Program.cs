using System;
using System.IO;
using App.Metrics.AspNetCore.Health;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace Sandbox.Api
{
    public sealed class Program
    {
        public static int Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();

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

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseKestrel(options => options.AddServerHeader = false)
                   .UseContentRoot(Directory.GetCurrentDirectory())
                   .ConfigureDefaultAppConfiguration(args)
                   .UseDefaultMetrics()
                   .UseSerilog()
                   .UseHealth()
                   .UseStartup<Startup>();
    }
}