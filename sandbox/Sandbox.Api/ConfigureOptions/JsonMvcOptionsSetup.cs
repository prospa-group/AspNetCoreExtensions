using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Converters;
using Prospa.Extensions.AspNetCore.Mvc.Core;

namespace Sandbox.Api.ConfigureOptions
{
    public class JsonMvcOptionsSetup : IConfigureOptions<MvcJsonOptions>
    {
        /// <inheritdoc />
        public void Configure(MvcJsonOptions options)
        {
            options.SerializerSettings.DateParseHandling = DefaultCamelCaseJsonSerializerSettings.Instance.DateParseHandling;
            options.SerializerSettings.MaxDepth = DefaultCamelCaseJsonSerializerSettings.Instance.MaxDepth;
            options.SerializerSettings.ContractResolver = DefaultCamelCaseJsonSerializerSettings.Instance.ContractResolver;
            options.SerializerSettings.Converters = DefaultCamelCaseJsonSerializerSettings.Instance.Converters;
            options.SerializerSettings.NullValueHandling = DefaultCamelCaseJsonSerializerSettings.Instance.NullValueHandling;
            options.SerializerSettings.MissingMemberHandling = DefaultCamelCaseJsonSerializerSettings.Instance.MissingMemberHandling;
            options.SerializerSettings.TypeNameHandling = DefaultCamelCaseJsonSerializerSettings.Instance.TypeNameHandling;
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
        }
    }
}
