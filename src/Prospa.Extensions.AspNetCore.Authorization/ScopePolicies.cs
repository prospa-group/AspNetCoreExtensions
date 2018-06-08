using System.Collections.Generic;
using System.Linq;

namespace Prospa.Extensions.AspNetCore.Authorization
{
    public class ScopePolicies : Dictionary<string, string[]>
    {
        public void AddScopePolicy(string policyName, string[] scopes) { Add(policyName, scopes); }

        public void AddScopePolicy(string policyName, string scope) { Add(policyName, new[] { scope }); }

        public IEnumerable<string> PolicyNames => Keys.ToArray();

        public IEnumerable<string> GetPolicyScopes(string policyName)
        {
            return !ContainsKey(policyName) ? Enumerable.Empty<string>() : this[policyName];
        }

        public IEnumerable<string> AllScopes() { return this.SelectMany(s => s.Value); }
    }
}
