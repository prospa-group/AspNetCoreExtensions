namespace Prospa.Extensions.AspNetCore.Authorization
{
    public static class Constants
    {
        public static readonly string Audience = nameof(AuthOptions.Audience);
        public static readonly string Authority = nameof(AuthOptions.Authority);

        public static class Claims
        {
            public const string Scope = "scope";
        }
    }
}
