using System.Linq;
using System.Threading.Tasks;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Authorization
    // ReSharper restore CheckNamespace
{
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == Prospa.Extensions.AspNetCore.Authorization.Constants.Claims.Scope))
            {
                return Task.CompletedTask;
            }

            var scopes = context.User.FindAll(c => c.Type == Prospa.Extensions.AspNetCore.Authorization.Constants.Claims.Scope);

            if (scopes.Any(s => s.Value == requirement.Scope))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
