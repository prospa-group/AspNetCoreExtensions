using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Prospa.Extensions.AspNet.WebApi.Owin.Middlewares
{
    public class RequireEndpointKeyMiddleware : OwinMiddleware
    {
        private static readonly string[] DefaultEndpoints = { "/health", "/metrics", "/metrics-text", "/env", "/docs" };
        private static readonly string DefaultKeyName = "EndpointKey";
        private readonly RequireEndpointKeyOptions _options;

        public RequireEndpointKeyMiddleware(OwinMiddleware next, RequireEndpointKeyOptions options)
            : base(next)
        {
            if (string.IsNullOrEmpty(options.Key))
            {
                throw new ArgumentNullException("Key", "The key for RequireEndpointKeyOptions is required");
            }

            _options = options;
        }

        private IList<string> Endpoints => (_options.Endpoints ?? DefaultEndpoints).ToList();

        private string KeyName => _options.KeyName ?? DefaultKeyName;

        public override async Task Invoke(IOwinContext context)
        {
            var key = ExtractToken(context);

            if (Endpoints.Contains(context.Request.Path.Value))
            {
                if (key != _options.Key)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    context.Response.ReasonPhrase = "Forbidden";
                    return;
                }
            }

            await Next.Invoke(context);
        }

        private string ExtractToken(IOwinContext context)
        {
            var endpointKey = context.Request.Query
                                     .FirstOrDefault(x => KeyName.Equals(x.Key, StringComparison.InvariantCultureIgnoreCase));

            return endpointKey.Value?.First() ?? string.Empty;
        }
    }
}
