using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Prospa.Extensions.AspNetCore.Mvc.Versioning.Swagger.OperationFilters
{
    public class DeprecatedVersionOperationFilter : IOperationFilter
    {
        /// <inheritdoc />
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var lastControllerVersion =
                context.ApiDescription.ControllerAttributes()
                .OfType<ApiVersionAttribute>().Where(apiVer => apiVer.Deprecated);

            if (!lastControllerVersion.Any())
            {
                return;
            }

            operation.Deprecated = true;
        }
    }
}
