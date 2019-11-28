using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace Prospa.Extensions.AspNetCore.Mvc.Versioning.Swagger.Extensions
{
    public static class ApiVersionExtensions
    {
        public static string ToInfoDisplay(this ApiVersion version)
        {
            return $"Version {version}";
        }

        public static string VersionFromInfo(this OpenApiInfo info)
        {
            return info.Version.Replace("Version ", string.Empty);
        }
    }
}