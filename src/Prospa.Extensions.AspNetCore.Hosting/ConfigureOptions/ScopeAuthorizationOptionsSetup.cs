using System;
using Microsoft.AspNetCore.Authorization;
using Prospa.Extensions.AspNetCore.Authorization;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.Options
    // ReSharper restore CheckNamespace
{
    public class ScopeAuthorizationOptionsSetup : IConfigureNamedOptions<AuthorizationOptions>
    {
        private readonly AuthOptions _options;

        public ScopeAuthorizationOptionsSetup(AuthOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void Configure(string name, AuthorizationOptions options)
        {
            foreach (var scopePolicy in _options.ScopePolicies.PolicyNames)
            {
                foreach (var scope in _options.ScopePolicies.GetPolicyScopes(scopePolicy))
                {
                    options.AddPolicy(scopePolicy, policy => { policy.Requirements.Add(new HasScopeRequirement(scope)); });
                }
            }
        }

        public void Configure(AuthorizationOptions options)
        {
            Configure(Options.DefaultName, options);
        }
    }
}