using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Prospa.Extensions.AspNetCore.Swagger.OperationFilters
{
    public class DelimitedQueryStringOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (HasNotDelimitedQueryStringAttribute(context.ApiDescription.ActionDescriptor))
            {
                return;
            }

            operation.Parameters?.Where(p => p.In.Equals("query", StringComparison.OrdinalIgnoreCase)).OfType<NonBodyParameter>().ToList().
                      ForEach(parameter => ApplyCsvCollectionFormat(context, parameter));
        }

        private static void ApplyCsvCollectionFormat(OperationFilterContext context, NonBodyParameter parameter)
        {
            var apiParam = context.ApiDescription.ParameterDescriptions.First(
                x => string.Equals(x.Name, parameter.Name, StringComparison.OrdinalIgnoreCase));

            if (apiParam.ModelMetadata.IsEnumerableType)
            {
                parameter.CollectionFormat = "csv";
            }
        }

        private static bool HasNotDelimitedQueryStringAttribute(ActionDescriptor actionDescriptor)
        {
            var controllerActionDescriptor = actionDescriptor as ControllerActionDescriptor;
            return controllerActionDescriptor?.MethodInfo.GetCustomAttribute<DelimitedQueryStringAttribute>() == null;
        }
    }
}