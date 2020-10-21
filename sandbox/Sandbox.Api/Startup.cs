using CorrelationId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Sandbox.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRequireHttps()
               .UseDefaultHealth()
               .UseCorrelationId()
               .UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All })
               .UseProspaDefaultSwagger();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseProspaDefaultDiagnostics(_hostingEnvironment);
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                    .AddProspaDefaultFluentValidation(typeof(Startup))
                    .AddApiExplorer()
                    .AddDataAnnotations();

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddProspaDefaultServices(_configuration, typeof(Startup));
            services.AddDefaultHealth(_configuration);

            services.AddHttpLoggingDelegatingHandler(options =>
            {
                // if a 401 or 400 is returned from an API, log an warning.
                // otherwise it as an Error.
                options.UnsuccessfulResponseLogLevel = LogLevel.Error;
                options.StatusCodeLogLevelOverrides[System.Net.HttpStatusCode.BadRequest] = LogLevel.Warning;
                options.StatusCodeLogLevelOverrides[System.Net.HttpStatusCode.Unauthorized] = LogLevel.Warning;
            });
        }
    }
}