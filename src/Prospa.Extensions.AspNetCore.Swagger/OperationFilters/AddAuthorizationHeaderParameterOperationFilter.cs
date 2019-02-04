using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
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
            var actionAttributes = context.MethodInfo.GetCustomAttributes(true);
            var controllerAttributes = context.MethodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true);
            var actionAndControllerAttributes = actionAttributes.Union(controllerAttributes).Distinct().ToList();
            var actionAndControllerPolicies = actionAndControllerAttributes.OfType<AuthorizeAttribute>().Select(attr => attr.Policy);

            if (!actionAndControllerPolicies.Any())
            {
                return;
            }

            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

            var requiredScopes = new List<string>();

            foreach (var policy in actionAndControllerPolicies)
            {
                requiredScopes.AddRange(GetPolicyScopes(policy));
            }

            operation.Description += $"\n\r<b>Required OAuth2 Scopes:</b> <i>{string.Join(", ", requiredScopes)}</i>";

            var oauthScheme = new OpenApiSecurityScheme
                              {
                                  Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                              };

            operation.Security = new List<OpenApiSecurityRequirement>
                                 {
                                     new OpenApiSecurityRequirement
                                     {
                                         [oauthScheme] = requiredScopes.ToList()
                                     }
                                 };

            operation.Parameters.Add(
                new OpenApiParameter
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "Bearer {access token}",
                    Required = true,
                    Style = ParameterStyle.Simple
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
