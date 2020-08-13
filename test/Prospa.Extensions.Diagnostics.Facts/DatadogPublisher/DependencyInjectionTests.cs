using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prospa.Extensions.Diagnostics.DDPublisher;
using Xunit;

namespace Prospa.Extensions.Diagnostics.Facts.DatadogPublisher
{
    public class DependencyInjectionTests
    {
        [Fact]
        public void Add_healthcheck()
        {
            var services = new ServiceCollection();

            services.AddHealthChecks()
                    .AddDatadogPublisher(
                        configuration =>
                        {
                            configuration.ServiceCheckName = "ServiceName";
                            configuration.ApiKey = "ApiKey";
                            configuration.Url = "Url";
                            configuration.ApplicationKey = "ApplicationKey";
                            configuration.Domain = "test";
                            configuration.Application = "app";
                            configuration.Environment = "test";
                        });

            var serviceProvider = services.BuildServiceProvider();
            var publisher = serviceProvider.GetService<IHealthCheckPublisher>();

            Assert.NotNull(publisher);
        }
    }
}
