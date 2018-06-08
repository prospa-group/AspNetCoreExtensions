using System;
using CorrelationId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sandbox.Api
{
    public class Startup : IStartup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
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
               .UseAuthorizationByPass()
               .UseAuthentication()
               .UseDefaultDiagnostics(_hostingEnvironment)
               .UseCors(Constants.Cors.AllowAny)
               .UseMvc()
               .UseDefaultSwagger()
               .UseDefaultSwaggerUi();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            AddCoreServices(services);
            AddApplicationServices(services);

            return services.BuildServiceProvider();
        }

        private void AddApplicationServices(IServiceCollection services)
        {
            // TODO: Add app specific services
        }

        private void AddCoreServices(IServiceCollection services)
        {
            services.AddCorrelationId();

            services.AddMvcCore()
                    .AddMetricsCore()
                    .AddDefaultCors()
                    .AddDefaultValidation()
                    .AddDefaultVersionedApiExplorer()
                    .AddAuthorization()
                    .AddDataAnnotations()
                    .AddJsonFormatters()
                    .AddDefaultJsonOptions()
                    .AddDefaultMvcOptions();

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