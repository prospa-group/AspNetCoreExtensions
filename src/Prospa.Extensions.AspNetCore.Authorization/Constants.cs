namespace Prospa.Extensions.AspNetCore.Authorization
{
    public static class Constants
    {
        public static readonly string Audience = nameof(AuthOptions.Audience);
        public static readonly string Authority = nameof(AuthOptions.Authority);
        public static readonly string BypassEnabled = nameof(AuthOptions.BypassAuthEnabled);
        public static readonly string DefaultBypassAuthType = "NoAuth";
        public static readonly string DefaultBypassAuthConfigPath = "/noauth";

        public static class Claims
        {
            public const string Scope = "scope";
        }
    }
}
