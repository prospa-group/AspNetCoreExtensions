using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
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
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var controllerScopes = context.ApiDescription.ControllerAttributes().OfType<AuthorizeAttribute>().Select(attr => attr.Policy);

            var actionPolicies = context.ApiDescription.ActionAttributes().OfType<AuthorizeAttribute>().Select(attr => attr.Policy);

            var requiredPolicies = controllerScopes.Union(actionPolicies).Distinct().ToList();

            if (!requiredPolicies.Any())
            {
                return;
            }

            operation.Responses.Add("401", new Response { Description = "Unauthorized" });
            operation.Responses.Add("403", new Response { Description = "Forbidden" });

            var requiredScopes = new List<string>();

            foreach (var policy in requiredPolicies)
            {
                requiredScopes.AddRange(GetPolicyScopes(policy));
            }

            operation.Description += $"\n\r<b>Required OAuth2 Scopes:</b> <i>{string.Join(", ", requiredScopes)}</i>";

            // Only setting this to get the padlock display
            operation.Security = new List<IDictionary<string, IEnumerable<string>>>
                                 {
                                     new Dictionary<string, IEnumerable<string>>
                                     {
                                         { "oauth2", requiredScopes }
                                     }
                                 };

            operation.Parameters.Add(
                new NonBodyParameter
                {
                    Name = "Authorization",
                    In = "header",
                    Description = "Bearer {access token}",
                    Required = true,
                    Type = "string"
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
