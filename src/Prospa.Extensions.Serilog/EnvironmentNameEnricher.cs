using System;
using Serilog.Events;

// ReSharper disable CheckNamespace
namespace Serilog.Core
    // ReSharper restore CheckNamespace
{
    public class EnvironmentNameEnricher : ILogEventEnricher
    {
        private const string AspNetCoreEnvVar = "ASPNETCORE_ENVIRONMENT";
        private const string EnvironmentPropertyName = "Environment";

        private readonly LogEventProperty _cachedProperty;

        public EnvironmentNameEnricher()
        {
            var aspnetCoreEnv = Environment.GetEnvironmentVariable(AspNetCoreEnvVar);

            if (string.IsNullOrWhiteSpace(aspnetCoreEnv))
            {
#if DEBUG
                aspnetCoreEnv = "Debug";
#else
                aspnetCoreEnv = "Release";
#endif
            }

            _cachedProperty = new LogEventProperty(EnvironmentPropertyName, new ScalarValue(aspnetCoreEnv));
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(_cachedProperty);
        }
    }
}