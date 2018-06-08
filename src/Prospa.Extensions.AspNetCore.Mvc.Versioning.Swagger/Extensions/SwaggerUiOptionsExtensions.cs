using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

// ReSharper disable CheckNamespace
namespace Swashbuckle.AspNetCore.SwaggerUI
    // ReSharper restore CheckNamespace
{
    public static class SwaggerUiOptionsExtensions
    {
        public static SwaggerUIOptions SwaggerVersionedJsonEndpoints(
            this SwaggerUIOptions options,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            foreach (var apiVersionDescription in apiVersionDescriptionProvider.ApiVersionDescriptions.OrderByDescending(x => x.ApiVersion))
            {
                options.SwaggerEndpoint(
                    $"/swagger/{apiVersionDescription.GroupName}/swagger.json",
                    $"Version {apiVersionDescription.ApiVersion}");
            }

            return options;
        }
    }
}