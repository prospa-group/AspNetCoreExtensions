using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Prospa.Extensions.Diagnostics.DDPublisher
{
    public static class HealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddDatadogPublisher(this IHealthChecksBuilder builder, Action<DatadogConfiguration> configAction)
        {
            builder.Services.AddSingleton<IHealthCheckPublisher>(
                provider =>
                {
                    var config = new DatadogConfiguration();
                    configAction.Invoke(config);

                    var context = new ValidationContext(config);

                    Validator.ValidateObject(config, context, true);

                    return new DatadogPublisher(config, new DatadogHttpClient(config));
                });

            return builder;
        }
    }
}
