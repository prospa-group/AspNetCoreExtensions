using System.Reflection;
using Serilog.Events;

// ReSharper disable CheckNamespace
namespace Serilog.Core
    // ReSharper restore CheckNamespace
{
    public class ApplicationVersionEnricher : ILogEventEnricher
    {
        public const string AssemblyVersionPropertyName = "ApplicationVersion";

        private readonly LogEventProperty _cachedProperty;

        public ApplicationVersionEnricher()
        {
            var entryAssembly = Assembly.GetEntryAssembly();

            if (entryAssembly == null)
            {
                return;
            }

            var version = entryAssembly.GetName().Version.ToString();

            if (string.IsNullOrWhiteSpace(version))
            {
                version = "unknown";
            }

            _cachedProperty = new LogEventProperty(AssemblyVersionPropertyName, new ScalarValue(version));
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(_cachedProperty);
        }
    }
}
