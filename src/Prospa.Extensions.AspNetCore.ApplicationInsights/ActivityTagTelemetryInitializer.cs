using System.Diagnostics;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Prospa.Extensions.AspNetCore.ApplicationInsights
{
    [DebuggerStepThrough]
    public class ActivityTagTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            var current = Activity.Current;

            if (telemetry is ISupportProperties dep)
            {
                while (current != null)
                {
                    foreach (var tag in current.Tags)
                    {
                        if (!dep.Properties.ContainsKey(tag.Key))
                        {
                            dep.Properties.Add(tag.Key, tag.Value);
                        }
                    }

                    foreach (var bag in current.Baggage)
                    {
                        if (!dep.Properties.ContainsKey(bag.Key))
                        {
                            dep.Properties.Add(bag.Key, bag.Value);
                        }
                    }

                    current = current.Parent;
                }
            }
        }
    }
}