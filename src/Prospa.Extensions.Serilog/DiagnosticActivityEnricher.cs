using System.Diagnostics;
using Serilog.Events;

// ReSharper disable CheckNamespace
namespace Serilog.Core
// ReSharper restore CheckNamespace
{
    public class DiagnosticActivityEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var activity = Activity.Current;

            while (activity != null)
            {
                foreach (var tag in activity.Tags)
                {
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(tag.Key, tag.Value));
                }

                foreach (var tag in activity.Baggage)
                {
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(tag.Key, tag.Value));
                }

                activity = activity.Parent;
            }
        }
    }
}