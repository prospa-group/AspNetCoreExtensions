using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Prospa.Extensions.AspNetCore.Mvc.Versioning.Swagger.OperationFilters
{
    public class DeprecatedVersionOperationFilter : IOperationFilter
    {
        /// <inheritdoc />
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var controllerAttributes = context.MethodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true);

            var lastControllerVersion = controllerAttributes.OfType<ApiVersionAttribute>().Where(apiVer => apiVer.Deprecated);

            if (!lastControllerVersion.Any())
            {
                return;
            }

            operation.Deprecated = true;
        }
    }
}
