namespace Prospa.Extensions.AspNetCore.Serilog
{
    public static class Constants
    {
        public static class HeaderKeys
        {
            public static readonly string CorrelationId = "X-Correlation-ID";

            public static readonly string OriginalFor = "X-Original-For";
        }

        public static class LogEventProperties
        {
            public static readonly string CorrelationId = nameof(CorrelationId);

            public static readonly string OriginalFor = nameof(OriginalFor);

            public static readonly string Sub = nameof(Sub);

            public static readonly string ClientId = nameof(ClientId);
        }
    }
}