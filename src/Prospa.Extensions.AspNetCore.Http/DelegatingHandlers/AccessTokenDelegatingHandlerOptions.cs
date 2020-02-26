namespace Prospa.Extensions.AspNetCore.Http.DelegatingHandlers
{
    public class AccessTokenDelegatingHandlerOptions
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Scope { get; set; }

        public string TokenEndpoint { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    }
}