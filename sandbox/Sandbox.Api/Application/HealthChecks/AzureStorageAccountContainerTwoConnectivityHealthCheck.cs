using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sandbox.Api.Application.HealthChecks
{
    public class AzureStorageAccountContainerTwoConnectivityHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var nowSecs = DateTime.UtcNow.Second;

            if (nowSecs % 2 == 0)
            {
                return Task.FromResult(HealthCheckResult.Healthy("container two connectivity healthy"));
            }

            if (nowSecs % 3 == 0)
            {
                return Task.FromResult(HealthCheckResult.Degraded("container two connectivity degraded"));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("container two connectivity unhealthy"));
        }
    }
}