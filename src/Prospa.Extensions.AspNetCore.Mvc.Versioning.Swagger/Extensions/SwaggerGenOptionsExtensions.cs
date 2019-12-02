using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Prospa.Extensions.AspNetCore.Mvc.Versioning.Swagger.Extensions;

// ReSharper disable CheckNamespace
namespace Swashbuckle.AspNetCore.SwaggerGen
    // ReSharper restore CheckNamespace
{
    public static class SwaggerGenOptionsExtensions
    {
        public static SwaggerGenOptions AllowFilteringDocsByApiVersion(this SwaggerGenOptions options)
        {
            options.DocInclusionPredicate((docName, apiDesc) =>
            {
                if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo))
                {
                    return false;
                }

                var versions = methodInfo.DeclaringType
                                         .GetCustomAttributes(true)
                                         .OfType<ApiVersionAttribute>()
                                         .SelectMany(attr => attr.Versions);

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

                var info = new OpenApiInfo
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