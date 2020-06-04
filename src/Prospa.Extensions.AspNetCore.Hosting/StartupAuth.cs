using System;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Prospa.Extensions.AspNetCore.Authorization;

namespace Prospa.Extensions.AspNetCore.Hosting
{
    public static class StartupAuth
    {
        public static IServiceCollection AddProspaDefaultAuthenticationAndAuthorization(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configure Auth Options
            services.Configure<AuthOptions>(configuration.GetSection(nameof(AuthOptions)));
            services.AddSingleton(provider => provider.GetRequiredService<IOptions<AuthOptions>>().Value);

            // Add Authorization
            services.AddAuthorization();
            services.AddSingleton<IConfigureOptions<AuthorizationOptions>, ScopeAuthorizationOptionsSetup>();
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            // Add Authentication
            var authOptions = new AuthOptions();
            configuration.GetSection(nameof(AuthOptions)).Bind(authOptions);
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(
                        options =>
                        {
                            options.Authority = authOptions.Authority;
                            options.ApiName = authOptions.Audience;
                            options.SupportedTokens = SupportedTokens.Both;
                            options.JwtValidationClockSkew = TimeSpan.FromMinutes(5);
                        });

            return services;
        }
    }
}