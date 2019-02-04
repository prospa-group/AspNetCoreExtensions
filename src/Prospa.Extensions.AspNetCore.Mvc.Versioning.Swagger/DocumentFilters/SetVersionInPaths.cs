using System.Linq;
using Microsoft.OpenApi.Models;
using Prospa.Extensions.AspNetCore.Mvc.Versioning.Swagger.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Prospa.Extensions.AspNetCore.Mvc.Versioning.Swagger.DocumentFilters
{
    public class SetVersionInPaths : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = swaggerDoc.Paths.ToDictionary(
                path => path.Key.Replace("{version}", swaggerDoc.Info.VersionFromInfo()),
                path => path.Value);

            swaggerDoc.Paths = new OpenApiPaths();

            foreach (var path in paths)
            {
                swaggerDoc.Paths.Add(path.Key, path.Value);
            }
        }
    }
}