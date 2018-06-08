using Prospa.Extensions.AspNetCore.Authorization.Middleware;

namespace Prospa.Extensions.AspNetCore.Authorization
{
    public class AuthOptions
    {
        public AuthOptions() { ScopePolicies = new ScopePolicies(); }

        public string Audience { get; set; }

        public string Authority { get; set; }

        /// <summary>
        ///     The authentication type to use when auth is bypassed. Defaults to
        ///     <see cref="Constants.DefaultBypassAuthType" />.
        /// </summary>
        public string ByPassAuthAuthType { get; set; } = Constants.DefaultBypassAuthType;

        /// <summary>
        ///     <value>true</value>
        ///     to enable endpoint to enable and disable auth bypass,
        ///     <value>false</value> otherwise. Defaults to <value>false</value>
        /// </summary>
        public bool BypassAuthEnabled { get; set; } = false;

        /// <summary>
        ///     The relative path of the endpoint exposed in <see cref="ByPassAuthorizationMiddleware" />. Defaults to
        ///     <see cref="Constants.DefaultBypassAuthConfigPath" />.
        /// </summary>
        public string BypassConfigPath { get; set; } = Constants.DefaultBypassAuthConfigPath;

        /// <summary>
        ///     The policies to register for scope authorization.
        /// </summary>
        public ScopePolicies ScopePolicies { get; set; }
    }
}