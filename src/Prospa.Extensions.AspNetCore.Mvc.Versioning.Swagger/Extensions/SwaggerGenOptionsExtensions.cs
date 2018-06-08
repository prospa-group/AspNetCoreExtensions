using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Prospa.Extensions.AspNetCore.Mvc.Versioning.Swagger.Extensions;
using Swashbuckle.AspNetCore.Swagger;

// ReSharper disable CheckNamespace
namespace Swashbuckle.AspNetCore.SwaggerGen
    // ReSharper restore CheckNamespace
{
    public static class SwaggerGenOptionsExtensions
    {
        public static SwaggerGenOptions AllowFilteringDocsByApiVersion(this SwaggerGenOptions options)
        {
            options.DocInclusionPredicate(
                (docName, apiDesc) =>
                {
                    var versions = apiDesc.ControllerAttributes().OfType<ApiVersionAttribute>().SelectMany(attr => attr.Versions);
                    return versions.Any(v => $"v{v.ToString()}" == docName);
                });

            return options;
        }

        public static SwaggerGenOptions SwaggerVersionedDoc(
            this SwaggerGenOptions options,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider,
            string assemblyDescription,
            string assemblyProduct)
        {
            foreach (var apiVersionDescription in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                var description = apiVersionDescription.IsDeprecated
                    ? $"{assemblyDescription} This API version has been deprecated."
                    : assemblyDescription;

                var info = new Info
                           {
                               Title = assemblyProduct,
                               Description = description,
                               Version = apiVersionDescription.ApiVersion.ToInfoDisplay()
                           };

                options.SwaggerDoc(apiVersionDescription.GroupName, info);
            }

            return options;
        }
    }
}