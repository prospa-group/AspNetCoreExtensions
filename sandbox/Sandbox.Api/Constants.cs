namespace Sandbox.Api
{
    public static class Constants
    {
        public static string HealthEndpoint = "/health";
        
        public static class Auth
        {
            public static class Policies
            {
                public const string ReadPolicy = "Read";
                public const string WritePolicy = "Write";
            }
        }

        public static class ConfigurationKeys
        {
            public static class Seq
            {
                public const string SeqServerUrl = nameof(SeqServerUrl);
            }
        }
    }
}