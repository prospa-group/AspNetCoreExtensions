using System.Security.Claims;
using System.Security.Principal;
using IdentityModel;
using Prospa.Extensions.AspNetCore.Serilog;
using Serilog.Events;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Http
    // ReSharper restore CheckNamespace
{
    public static class LogEventPropertyIdentityExtensions
    {
        public static LogEventProperty SubjectLogEventProperty(this IIdentity identity)
        {
            if (!identity.IsAuthenticated)
            {
                return null;
            }

            var id = identity as ClaimsIdentity;

            if (id == null)
            {
                return null;
            }

            var claim = id.FindFirst(JwtClaimTypes.Subject);

            if (claim == null)
            {
                claim = id.FindFirst(ClaimTypes.NameIdentifier);
            }

            if (claim == null)
            {
                return null;
            }

            return new LogEventProperty(Constants.LogEventProperties.Subject, new ScalarValue(claim.Value));
        }
    }
}