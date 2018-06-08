using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Prospa.Extensions.AspNetCore.Mvc.Versioning.Swagger.OperationFilters
{
    public class RemoveVersionParameters : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var versionParameter = operation.Parameters?.FirstOrDefault(p => p.Name == "version");

            if (versionParameter == null)
            {
                return;
            }

            operation.Parameters.Remove(versionParameter);
        }
    }
}
