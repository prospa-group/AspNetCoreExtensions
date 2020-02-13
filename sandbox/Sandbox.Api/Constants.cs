using System;
using Microsoft.AspNetCore.HttpOverrides;

namespace Sandbox.Api
{
    public static class Constants
    {
        public static class Auth
        {
            public static readonly string EndpointKey = nameof(EndpointKey);

            public static class Policies
            {
                public const string ReadPolicy = "Read";
                public const string WritePolicy = "Write";
            }
        }

        public static class ConfigurationKeys
        {
            public static class AppInsights
            {
                public const string InstrumentationKey = "APPINSIGHTS_INSTRUMENTATIONKEY";
            }

            public static class Seq
            {
                public const string SeqServerUrl = nameof(SeqServerUrl);
            }
        }

        public static class Cors
        {
            public const string AllowAny = nameof(AllowAny);
        }

        public const string EndpointKey = "A8559AC2-6E35-46F0-BDB4-4F1FAA2990F0";

        public static class Environments
        {
            public const string Development = nameof(Development);
            public const string Production = nameof(Production);
            public const string Staging = nameof(Staging);
            public static readonly string CurrentAspNetCoreEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            public static bool IsDevelopment() { return CurrentAspNetCoreEnv == Development; }
        }

        public static class HttpHeaders
        {
            public const int HstsMaxAgeDays = 18 * 7;
            public const ForwardedHeaders ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All;
        }

        public static class Versioning
        {
            public const string GroupNameFormat = "'v'V";
        }
    }
}