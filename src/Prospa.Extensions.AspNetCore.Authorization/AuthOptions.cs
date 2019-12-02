namespace Prospa.Extensions.AspNetCore.Authorization
{
    public class AuthOptions
    {
        public AuthOptions() { ScopePolicies = new ScopePolicies(); }

        public string Audience { get; set; }

        public string Authority { get; set; }

        /// <summary>
        ///     The policies to register for scope authorization.
        /// </summary>
        public ScopePolicies ScopePolicies { get; set; }
    }
}