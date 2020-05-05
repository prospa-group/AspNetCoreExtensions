using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Prospa.Extensions.AspNetCore.Authorization;

namespace Prospa.Extensions.AspNetCore.Hosting
{
    public static class StartupAuth
    {
        public static IServiceCollection AddDefaultAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthOptions>(configuration.GetSection(nameof(AuthOptions)));
            services.AddSingleton(provider => provider.GetRequiredService<IOptions<AuthOptions>>().Value);

            services.AddDefaultAuthorization();
            services.AddDefaultAuthentication();

            return services;
        }

        public static IServiceCollection AddDefaultAuthorization(this IServiceCollection services)
        {
            services
                .AddAuthorization()
                .AddDefaultScopeAuthorization();

            return services;
        }

        public static IServiceCollection AddDefaultAuthentication(this IServiceCollection services)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddDefaultJwtBearer();

            return services;
        }

        public static IServiceCollection AddDefaultScopeAuthorization(this IServiceCollection services)
        {
            services.AddSingleton<IConfigureOptions<AuthorizationOptions>, ScopeAuthorizationOptionsSetup>();
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            return services;
        }

        public static AuthenticationBuilder AddDefaultJwtBearer(this AuthenticationBuilder builder)
        {
            builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, JwtBearerOptionsSetup>();
            builder.AddJwtBearer();

            return builder;
        }
    }
}