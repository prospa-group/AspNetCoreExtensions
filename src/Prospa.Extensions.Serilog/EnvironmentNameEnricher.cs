using System;
using Serilog.Events;

// ReSharper disable CheckNamespace
namespace Serilog.Core
    // ReSharper restore CheckNamespace
{
    public class EnvironmentNameEnricher : ILogEventEnricher
    {
        private const string AspNetCoreEnvVar = "ASPNETCORE_ENVIRONMENT";
        private const string NetCoreEnvVar = "DOTNET_ENVIRONMENT";
        private const string EnvironmentPropertyName = "Environment";

        private readonly LogEventProperty _cachedProperty;

        public EnvironmentNameEnricher()
        {
            var env = Environment.GetEnvironmentVariable(AspNetCoreEnvVar);

            if (string.IsNullOrWhiteSpace(env))
            {
                env = Environment.GetEnvironmentVariable(NetCoreEnvVar);

                if (string.IsNullOrWhiteSpace(env))
                {
#if DEBUG
                    env = "Debug";
#else
                env = "Release";
#endif
                }
            }

            _cachedProperty = new LogEventProperty(EnvironmentPropertyName, new ScalarValue(env));
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(_cachedProperty);
        }
    }
}