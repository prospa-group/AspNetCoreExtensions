using System;
using Owin;
using Prospa.Extensions.AspNet.WebApi.Owin.Middlewares;

namespace Prospa.Extensions.AspNet.WebApi.Owin.Extensions
{
    public static class AppBuilderExtensions
    {
        public static void AddRequiredEndpointKey(this IAppBuilder app, Action<RequireEndpointKeyOptions> buildOptions)
        {
            var options = new RequireEndpointKeyOptions();

            buildOptions(options);

            app.Use<RequireEndpointKeyMiddleware>(options);
        }
    }
}
