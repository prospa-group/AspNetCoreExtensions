using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Sandbox.Api.ConfigureOptions
{
    public class JsonOptionsSetup : IConfigureOptions<JsonOptions>
    {
        private const int MaxJsonDepth = 32;

        /// <inheritdoc />
        public void Configure(JsonOptions options)
        {
            options.JsonSerializerOptions.MaxDepth = MaxJsonDepth;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        }
    }
}
