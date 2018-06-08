using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using Sandbox.Api;
using Sandbox.Api.ConfigureOptions;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class StartupMvc
    {
        public static IServiceCollection AddDefaultContextAccessors(this IServiceCollection services)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                    .AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }

        public static IMvcCoreBuilder AddDefaultCors(this IMvcCoreBuilder builder)
        {
            builder.Services.AddSingleton<IConfigureOptions<CorsOptions>, CorsOptionsSetup>();
            builder.AddCors();

            return builder;
        }

        public static IMvcCoreBuilder AddDefaultJsonOptions(this IMvcCoreBuilder builder)
        {
            builder.Services.AddSingleton<IConfigureOptions<MvcJsonOptions>, JsonMvcOptionsSetup>();

            return builder;
        }

        public static IMvcCoreBuilder AddDefaultMvcOptions(this IMvcCoreBuilder builder)
        {
            builder.SetCompatibilityVersion(CompatibilityVersion.Latest);
            builder.Services.AddSingleton<IConfigureOptions<MvcOptions>, FormattersMvcOptionsSetup>();

            return builder;
        }

        public static IMvcCoreBuilder AddDefaultVersionedApiExplorer(this IMvcCoreBuilder builder)
        {
            builder
                .AddApiExplorer()
                .AddVersionedApiExplorer(options => options.GroupNameFormat = Constants.Versioning.GroupNameFormat);

            return builder;
        }
    }
}