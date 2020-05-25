using CorrelationId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
               .UseProspaDefaultDiagnostics(_hostingEnvironment)
               .UseProspaDefaultSwagger();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                    .AddProspaDefaultFluentValidation(typeof(Startup))
                    .AddApiExplorer()
                    .AddAuthorization()
                    .AddDataAnnotations();

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddProspaDefaultServices(_configuration, typeof(Startup));
            services.AddDefaultHealth();
        }
    }
}