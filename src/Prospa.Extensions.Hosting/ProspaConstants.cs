using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Prospa.Extensions.Hosting
{
    public static class ProspaConstants
    {
        public static Task WriteHealthResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            var resultBytes = result.ToByteArray();

            var json = Encoding.UTF8.GetString(resultBytes);

            return context.Response.WriteAsync(json);
        }

        public static byte[] ToByteArray(this HealthReport report)
        {
            var options = new JsonWriterOptions { Indented = true };

            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream, options);

            writer.WriteStartObject();
            writer.WriteString("status", report.Status.ToString());
            writer.WriteStartObject("results");

            foreach (var entry in report.Entries)
            {
                writer.WriteStartObject(entry.Key);
                writer.WriteString("status", entry.Value.Status.ToString());
                writer.WriteString("description", entry.Value.Description);
                writer.WriteStartObject("data");

                foreach (var item in entry.Value.Data)
                {
                    writer.WritePropertyName(item.Key);
                    JsonSerializer.Serialize(writer, item.Value, item.Value?.GetType() ?? typeof(object));
                }

                writer.WriteEndObject();
                writer.WriteEndObject();
            }

            writer.WriteEndObject();
            writer.WriteEndObject();
            writer.Flush();
            
            return stream.ToArray();
        }

        public static class Environments
        {
            public static readonly string CurrentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
                                                       Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

            public static bool IsDevelopment => CurrentEnv == Microsoft.Extensions.Hosting.Environments.Development;

            public static bool IsProduction => CurrentEnv == Microsoft.Extensions.Hosting.Environments.Production;

            public static bool IsStaging => CurrentEnv == Microsoft.Extensions.Hosting.Environments.Staging;

            public static string CurrentEnvLogTag()
            {
                if (IsProduction)
                {
                    return "live";
                }

                if (IsStaging)
                {
                    return "staging";
                }

                return "demo";
            }

            public static string Prefix()
            {
                if (IsDevelopment)
                {
                    return "demo-";
                }

                if (IsStaging)
                {
                    return "staging-";
                }

                if (IsProduction)
                {
                    return "live-";
                }

                throw new ArgumentException("Invalid ASPNETCORE_ENVIRONMENT");
            }
        }

        public static class SharedConfigurationKeys
        {
            public const string SharedAzureAppConfigurationEndpoint = nameof(SharedAzureAppConfigurationEndpoint);

            public const string AzureServiceBusConnection = nameof(AzureServiceBusConnection);
        }
    }
}
