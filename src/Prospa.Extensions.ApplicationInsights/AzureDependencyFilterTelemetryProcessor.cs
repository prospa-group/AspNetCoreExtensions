using System.Diagnostics;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Prospa.Extensions.ApplicationInsights
{
    /// <summary>
    /// filters out dependencies like polling queues that are not attached to any larger operation.
    /// </summary>
    [DebuggerStepThrough]
    public class AzureDependencyFilterTelemetryProcessor : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor _inner;

        public AzureDependencyFilterTelemetryProcessor(ITelemetryProcessor inner)
        {
            _inner = inner;
        }

        public void Process(ITelemetry item)
        {
            if (item is Microsoft.ApplicationInsights.DataContracts.DependencyTelemetry dependency
                && dependency.Success == true
                && dependency.Context.Operation.Name == null
                && (dependency.Type == "Azure Service Bus"
                    || dependency.Type == "Azure table"
                    || dependency.Type == "Azure blob"
                    || dependency.Type == "Azure queue"))
            {
                return;
            }

            _inner.Process(item);
        }
    }
}