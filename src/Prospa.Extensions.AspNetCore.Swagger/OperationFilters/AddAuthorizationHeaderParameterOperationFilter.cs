using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Prospa.Extensions.AspNetCore.Swagger.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Prospa.Extensions.AspNetCore.Swagger.OperationFilters
{
    /// <summary>
    ///     Inspects the filter descriptors to look for authorization filters and if found an operation paramater to allow
    ///     passing of an access token.
    /// </summary>
    public class AddAuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        private readonly Dictionary<string, string[]> _scopePolicyMapping;

        public AddAuthorizationHeaderParameterOperationFilter(Dictionary<string, string[]> scopePolicyMapping) { _scopePolicyMapping = scopePolicyMapping; }

        /// <inheritdoc />
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var controllerScopes = context.GetControllerAndActionAttributes<AuthorizeAttribute>().Select(attr => attr.Policy);
            var requiredPolicies = controllerScopes.Distinct().ToList();

            if (!requiredPolicies.Any())
            {
                return;
            }

            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

            var requiredScopes = new List<string>();

            foreach (var policy in requiredPolicies)
            {
                requiredScopes.AddRange(GetPolicyScopes(policy));
            }

            operation.Description += $"\n\r<b>Required OAuth2 Scopes:</b> <i>{string.Join(", ", requiredScopes)}</i>";

            // Only setting this to get the padlock display
            operation.Security.Add(new OpenApiSecurityRequirement
                                   {
                                       { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" } }, requiredPolicies.ToList() }
                                   });

            operation.Parameters.Add(
                new OpenApiParameter
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "Bearer {access token}",
                    Required = true,
                    Schema = new OpenApiSchema
                             {
                                 Type = "string"
                             }
                });
        }

        public IEnumerable<string> GetPolicyScopes(string policyName)
        {
            if (string.IsNullOrWhiteSpace(policyName))
            {
                return Enumerable.Empty<string>();
            }

            return !_scopePolicyMapping.ContainsKey(policyName) ? Enumerable.Empty<string>() : _scopePolicyMapping[policyName];
        }
    }
}
