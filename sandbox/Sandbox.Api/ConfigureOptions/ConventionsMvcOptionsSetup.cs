using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.Options
    // ReSharper restore CheckNamespace
{
    public class ConventionsMvcOptionsSetup : IConfigureOptions<MvcOptions>
    {
        /// <inheritdoc />
        public void Configure(MvcOptions options)
        {
            options.Conventions.Add(new ApiVersionConvention());
        }
    }
}
