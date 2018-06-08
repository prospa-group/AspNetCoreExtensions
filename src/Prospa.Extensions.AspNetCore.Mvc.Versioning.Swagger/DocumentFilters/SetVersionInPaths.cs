using System.Linq;
using Prospa.Extensions.AspNetCore.Mvc.Versioning.Swagger.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Prospa.Extensions.AspNetCore.Mvc.Versioning.Swagger.DocumentFilters
{
    public class SetVersionInPaths : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Paths = swaggerDoc.Paths.ToDictionary(
                path => path.Key.Replace("{version}", swaggerDoc.Info.VersionFromInfo()),
                path => path.Value);
        }
    }
}