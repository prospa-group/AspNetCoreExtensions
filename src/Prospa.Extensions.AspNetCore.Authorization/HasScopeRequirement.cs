using System;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Authorization
    // ReSharper restore CheckNamespace
{
    public class HasScopeRequirement : IAuthorizationRequirement
    {
        public HasScopeRequirement(string scope)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        public string Scope { get; }
    }
}
