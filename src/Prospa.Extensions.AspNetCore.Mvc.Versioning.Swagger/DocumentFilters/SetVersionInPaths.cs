using Microsoft.OpenApi.Models;
using Prospa.Extensions.AspNetCore.Mvc.Versioning.Swagger.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Prospa.Extensions.AspNetCore.Mvc.Versioning.Swagger.DocumentFilters
{
    public class SetVersionInPaths : IDocumentFilter
    {
        /// <inheritdoc />
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = new OpenApiPaths();

            foreach (var path in swaggerDoc.Paths)
            {
                paths.Add(path.Key.Replace("{version}", swaggerDoc.Info.VersionFromInfo()), path.Value);
            }

            swaggerDoc.Paths = paths;
        }
    }
}