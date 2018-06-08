using Microsoft.Extensions.DependencyInjection;
using Prospa.Extensions.AspNetCore.Authorization;
using Prospa.Extensions.AspNetCore.Authorization.Middleware;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Builder
    // ReSharper restore CheckNamespace
{
    public static class AuthorizationByPassAppBuilderExtensions
    {
        /// <summary>
        ///     IMPORTANT: Only to be used to simplify testing e.g. stress and performance tests where authorization can be
        ///     bypassed by setting up a fake user.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         - To configure a fake user: /noauth?sub=123
        ///     </para>
        ///     <para>
        ///         - To reset the face user re-enabling authz: /noauth/reset
        ///     </para>
        /// </remarks>
        /// <param name="app">The application builder instance to configure require middleware</param>
        /// <returns>The instance of the application builder</returns>
        public static IApplicationBuilder UseAuthorizationByPass(this IApplicationBuilder app)
        {
            var authzOptions = app.ApplicationServices.GetRequiredService<AuthOptions>();

            if (authzOptions.BypassAuthEnabled)
            {
                app.UseMiddleware<ByPassAuthorizationMiddleware>();
            }

            return app;
        }
    }
}
