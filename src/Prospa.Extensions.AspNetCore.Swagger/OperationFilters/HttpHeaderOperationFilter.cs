using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Prospa.Extensions.AspNetCore.Mvc.Core.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Prospa.Extensions.AspNetCore.Swagger.OperationFilters
{
    public class HttpHeaderOperationFilter : IOperationFilter
    {
        /// <summary>
        ///     Applies the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="context">The context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var filter = context.ApiDescription
                                .ActionDescriptor
                                .FilterDescriptors
                                .Select(x => x.Filter)
                                .FirstOrDefault(x => x.GetType() == typeof(HttpHeaderAttribute) || x.GetType().IsSubclassOf(typeof(HttpHeaderAttribute))) as HttpHeaderAttribute;

            if (filter == null)
            {
                return;
            }

            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            var description = filter.Description;

            if (filter.Forward)
            {
                description += "<br />Will be sent back in the HTTP response headers.";
            }

            var noBodyParameter = new OpenApiParameter
                                  {
                                      Description = description,
                                      In = ParameterLocation.Header,
                                      Name = filter.HttpHeaderName,
                                      Required = filter.Required,
                                      Style = ParameterStyle.Simple
                                  };

            if (filter.GetType() == typeof(CorrelationIdHttpHeaderAttribute))
            {
                // TODO: OpenAPI
                // noBodyParameter.Default = Guid.NewGuid();
            }

            operation.Parameters.Add(noBodyParameter);
        }
    }
}