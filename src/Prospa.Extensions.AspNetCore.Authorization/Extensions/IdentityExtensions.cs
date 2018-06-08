using Prospa.Extensions.AspNetCore.Authorization;

// ReSharper disable CheckNamespace
namespace System.Security.Claims
    // ReSharper restore CheckNamespace
{
    public static class IdentityExtensions
    {
        public static ClaimsPrincipal BuildTestIdentityAllScopeAccess(
            this ClaimsPrincipal claimsPrincipal,
            AuthOptions authOptions,
            string subject)
        {
            var scopeClaim = new Claim(
                "scope",
                string.Join(" ", authOptions.ScopePolicies.AllScopes()),
                "string",
                authOptions.Authority);

            var identity = new ClaimsIdentity(
                new[]
                {
                    new Claim("sub", subject),
                    new Claim("aud", "http://localhost"),
                    new Claim("name", "Test User"),
                    scopeClaim,
                    new Claim("nonce", Guid.NewGuid().ToString())
                },
                authOptions.ByPassAuthAuthType);

            return new ClaimsPrincipal(identity);
        }
    }
}
