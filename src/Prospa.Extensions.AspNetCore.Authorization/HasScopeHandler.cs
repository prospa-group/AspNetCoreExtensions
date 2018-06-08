using System.Linq;
using System.Threading.Tasks;
using Prospa.Extensions.AspNetCore.Authorization;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Authorization
    // ReSharper restore CheckNamespace
{
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == Constants.Claims.Scope && c.Issuer == requirement.Issuer))
            {
                return Task.CompletedTask;
            }

            var scopes = context.User.FindFirst(c => c.Type == Constants.Claims.Scope && c.Issuer == requirement.Issuer).Value.Split(' ');

            if (scopes.Any(s => s == requirement.Scope))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
