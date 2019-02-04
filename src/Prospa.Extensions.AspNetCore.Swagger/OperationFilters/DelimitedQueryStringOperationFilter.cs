using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Prospa.Extensions.AspNetCore.Swagger.OperationFilters
{
    public class DelimitedQueryStringOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // TODO: OpenAPI
            // if (HasNotDelimitedQueryStringAttribute(context.ApiDescription.ActionDescriptor))
            // {
            //    return;
            // }

            // operation.Parameters?.Where(p => p.In.Equals(ParameterLocation.Query, StringComparison.OrdinalIgnoreCase)).OfType<OpenApiParameter>().ToList().
            //          ForEach(parameter => ApplyCsvCollectionFormat(context, parameter));
        }

        // private static void ApplyCsvCollectionFormat(OperationFilterContext context, NonBodyParameter parameter)
        // {
        //    var apiParam = context.ApiDescription.ParameterDescriptions.First(
        //        x => string.Equals(x.Name, parameter.Name, StringComparison.OrdinalIgnoreCase));

        // if (apiParam.ModelMetadata.IsEnumerableType)
        //    {
        //        parameter.CollectionFormat = "csv";
        //    }
        // }

        // private static bool HasNotDelimitedQueryStringAttribute(ActionDescriptor actionDescriptor)
        // {
        //    var controllerActionDescriptor = actionDescriptor as ControllerActionDescriptor;
        //    return controllerActionDescriptor?.MethodInfo.GetCustomAttribute<DelimitedQueryStringAttribute>() == null;
        // }
    }
}