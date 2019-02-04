using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Prospa.Extensions.AspNetCore.Swagger.SchemaFilters
{
    /// <summary>
    ///     Shows an example of a <see cref="ModelStateDictionary" /> containing errors.
    /// </summary>
    /// <seealso cref="ISchemaFilter" />
    public class ModelStateDictionarySchemaFilter : ISchemaFilter
    {
        /// <summary>
        ///     Applies the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="context">The context.</param>
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (context.SystemType == typeof(ModelStateDictionary))
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError("property1", "Error message 1");
                modelState.AddModelError("property2", "Error message 2");
                var serializableError = new SerializableError(modelState);

                // TODO: OpenAPI
                // model.Default = serializableError;
                // model.Example = serializableError;
            }
        }
    }
}