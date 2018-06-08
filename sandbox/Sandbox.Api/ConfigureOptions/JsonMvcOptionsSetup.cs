using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Converters;
using Prospa.Extensions.Http.Json;

namespace Sandbox.Api.ConfigureOptions
{
    public class JsonMvcOptionsSetup : IConfigureOptions<MvcJsonOptions>
    {
        /// <inheritdoc />
        public void Configure(MvcJsonOptions options)
        {
            options.SerializerSettings.DateParseHandling = DefaultJsonSerializerSettings.Instance.DateParseHandling;
            options.SerializerSettings.MaxDepth = DefaultJsonSerializerSettings.Instance.MaxDepth;
            options.SerializerSettings.ContractResolver = DefaultJsonSerializerSettings.Instance.ContractResolver;
            options.SerializerSettings.Converters = DefaultJsonSerializerSettings.Instance.Converters;
            options.SerializerSettings.NullValueHandling = DefaultJsonSerializerSettings.Instance.NullValueHandling;
            options.SerializerSettings.MissingMemberHandling = DefaultJsonSerializerSettings.Instance.MissingMemberHandling;
            options.SerializerSettings.Formatting = DefaultJsonSerializerSettings.Instance.Formatting;
            options.SerializerSettings.TypeNameHandling = DefaultJsonSerializerSettings.Instance.TypeNameHandling;
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
        }
    }
}
