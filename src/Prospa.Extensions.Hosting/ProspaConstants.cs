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

            var options = new JsonWriterOptions { Indented = true };

            using var stream = new MemoryStream();
            using (var writer = new Utf8JsonWriter(stream, options))
            {
                writer.WriteStartObject();
                writer.WriteString("status", result.Status.ToString());
                writer.WriteStartObject("results");

                foreach (var (key, value) in result.Entries)
                {
                    writer.WriteStartObject(key);
                    writer.WriteString("status", value.Status.ToString());
                    writer.WriteString("description", value.Description);
                    writer.WriteStartObject("data");

                    foreach (var item in value.Data)
                    {
                        writer.WritePropertyName(item.Key);
                        JsonSerializer.Serialize(writer, item.Value, item.Value?.GetType() ?? typeof(object));
                    }

                    writer.WriteEndObject();
                    writer.WriteEndObject();
                }

                writer.WriteEndObject();
                writer.WriteEndObject();
            }

            var json = Encoding.UTF8.GetString(stream.ToArray());

            return context.Response.WriteAsync(json);
        }

        public static class Environments
        {
            public static readonly string CurrentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
                                                       Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

            public static bool IsDevelopment => CurrentEnv == Microsoft.Extensions.Hosting.Environments.Development;

            public static bool IsProduction => CurrentEnv == Microsoft.Extensions.Hosting.Environments.Production;

            public static bool IsStaging => CurrentEnv == Microsoft.Extensions.Hosting.Environments.Staging;

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

                throw new ApplicationException("Invalid ASPNETCORE_ENVIRONMENT");
            }
        }

        public static class SharedConfigurationKeys
        {
            public const string AzureAppConfigurationEndpoint = nameof(AzureAppConfigurationEndpoint);

            public const string AzureServiceBusConnection = nameof(AzureServiceBusConnection);
        }
    }
}