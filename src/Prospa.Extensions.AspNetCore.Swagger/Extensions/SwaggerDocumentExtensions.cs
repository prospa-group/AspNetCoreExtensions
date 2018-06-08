using System.Linq;

// ReSharper disable CheckNamespace
namespace Swashbuckle.AspNetCore.Swagger
    // ReSharper restore CheckNamespace
{
    public static class SwaggerDocumentExtensions
    {
        public static SwaggerDocument LowercaseRoutes(this SwaggerDocument swagger)
        {
            var paths = swagger.Paths.ToDictionary(item => item.Key.ToLowerInvariant(), item => item.Value);
            swagger.Paths.Clear();

            foreach (var pathItem in paths)
            {
                swagger.Paths.Add(pathItem.Key, pathItem.Value);
            }

            return swagger;
        }
    }
}
