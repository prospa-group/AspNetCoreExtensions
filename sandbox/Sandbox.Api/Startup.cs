using System;
using CorrelationId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
               .UseCorrelationId(new CorrelationIdOptions { UpdateTraceIdentifier = false })
               .UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = Constants.HttpHeaders.ForwardedHeaders })
               .UseDefaultSecurityHeaders(_hostingEnvironment)
               .UseAuthentication()
               .UseAuthorization()
               .UseDefaultDiagnostics(_hostingEnvironment)
               .UseCors(Constants.Cors.AllowAny)
               .UseDefaultSwagger()
               .UseDefaultSwaggerUi();

            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            AddCoreServices(services);
            AddApplicationServices(services);
        }

        private void AddApplicationServices(IServiceCollection services)
        {
            // TODO: Add app specific services
        }

        private void AddCoreServices(IServiceCollection services)
        {
            services.AddCorrelationId();

            services.AddHealthChecks();

            services
                    .AddMvcCore()
                    .AddMetricsCore()
                    .AddDefaultCors()
                    .AddDefaultValidation()
                    .AddApiExplorer()
                    .AddAuthorization()
                    .AddDataAnnotations()
                    .AddDefaultJsonOptions()
                    .AddDefaultMvcOptions();

            services.AddRouting(options => options.LowercaseUrls = true);
            
            services.AddApiVersioning()
                    .AddVersionedApiExplorer(options => options.GroupNameFormat = Constants.Versioning.GroupNameFormat);

            services
                .AddRouting(options => options.LowercaseUrls = true)
                .AddDefaultAuthenticationAndAuthorization(_configuration)
                .AddDefaultApiVersioning()
                .AddDefaultContextAccessors()
                .AddDefaultDiagnostics(_configuration)
                .AddDefaultSwagger();
        }
    }
}