using System.Reflection;
using Serilog.Events;

// ReSharper disable CheckNamespace
namespace Serilog.Core
    // ReSharper restore CheckNamespace
{
    public class ApplicationNameEnricher : ILogEventEnricher
    {
        public const string AssemblyNamePropertyName = "ApplicationName";

        private readonly LogEventProperty _cachedProperty;

        public ApplicationNameEnricher()
        {
            var entryAssembly = Assembly.GetEntryAssembly();

            if (entryAssembly == null)
            {
                return;
            }

            var application = entryAssembly.GetName().Name;

            if (string.IsNullOrWhiteSpace(application))
            {
                application = "unknown";
            }

            _cachedProperty = new LogEventProperty(AssemblyNamePropertyName, new ScalarValue(application));
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(_cachedProperty);
        }
    }
}
