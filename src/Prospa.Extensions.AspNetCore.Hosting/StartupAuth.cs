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
        public static IServiceCollection AddProspaDefaultAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure Auth Options
            services.Configure<AuthOptions>(configuration.GetSection(nameof(AuthOptions)));
            services.AddSingleton(provider => provider.GetRequiredService<IOptions<AuthOptions>>().Value);

            // Add Authorization
            services.AddAuthorization();
            services.AddSingleton<IConfigureOptions<AuthorizationOptions>, ScopeAuthorizationOptionsSetup>();
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            // Add Authentication
            var authenticationBuilder = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
            authenticationBuilder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, JwtBearerOptionsSetup>();
            authenticationBuilder.AddJwtBearer();

            return services;
        }
    }
}