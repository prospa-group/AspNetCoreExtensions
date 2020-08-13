using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Sandbox.Api
{
    public sealed class Program
    {
        public static int Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();

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
            return Host.CreateDefaultBuilder(args)
                       .ConfigureAppConfiguration(
                           (context, builder) =>
                           {
                               builder.AddSharedAppConfiguration();
                           })
                       .ConfigureServices((context, services) =>
                       {
                           services.AddProspaMetaEndpointProtection( context, Constants.HealthEndpoint);
                       })
                       .UseSerilog((context, configuration) =>
                       {
                           context.CreateProspaDefaultLogger(configuration, typeof(Program));
                       })
                       .ConfigureWebHostDefaults(webHostBuilder =>
                       {
                           webHostBuilder
                               .ConfigureKestrel(options => { options.AddServerHeader = false; })
                               .UseStartup<Startup>();
                       });
        }
    }
}