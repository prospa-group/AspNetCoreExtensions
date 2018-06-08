using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Prospa.Extensions.AspNetCore.Authorization;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.Options
// ReSharper restore CheckNamespace
{
    public class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly AuthOptions _options;

        public JwtBearerOptionsSetup(AuthOptions options)
        {
            _options = options;
        }

        public void Configure(string name, JwtBearerOptions options)
        {
            options.Audience = _options.Audience;
            options.Authority = _options.Authority;
            options.IncludeErrorDetails = true;
            options.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(5);
        }

        public void Configure(JwtBearerOptions options)
        {
            Configure(Options.DefaultName, options);
        }
    }
}
