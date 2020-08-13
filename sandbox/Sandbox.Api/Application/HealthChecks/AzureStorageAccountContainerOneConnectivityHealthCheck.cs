using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sandbox.Api.Application.HealthChecks
{
    public class AzureStorageAccountContainerOneConnectivityHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var nowSecs = DateTime.UtcNow.Second;

            if (nowSecs % 2 == 0)
            {
                return Task.FromResult(HealthCheckResult.Healthy("container one connectivity healthy"));
            }

            if (nowSecs % 3 == 0)
            {
                return Task.FromResult(HealthCheckResult.Degraded("container one connectivity degraded"));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("container one connectivity unhealthy"));
        }
    }
}
