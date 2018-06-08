using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Prospa.Extensions.AspNetCore.Authorization.Middleware
{
    /// <summary>
    ///     IMPORTANT: Only to be used to simplify testing e.g. stress and performance tests where authorization can be
    ///     bypassed by setting up a fake user.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         - To configure a fake user: /noauth?sub=123
    ///     </para>
    ///     <para>
    ///         - To reset the face user re-enabling authz: /noauth/reset
    ///     </para>
    /// </remarks>
    public class ByPassAuthorizationMiddleware
    {
        private readonly AuthOptions _authOptions;
        private readonly RequestDelegate _next;
        private string _currentSub;

        public ByPassAuthorizationMiddleware(RequestDelegate next, AuthOptions authOptions)
        {
            _next = next;
            _authOptions = authOptions;
            _currentSub = null;
        }

        public Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;

            if (path == _authOptions.BypassConfigPath)
            {
                return SetSubjectFromQueryString(context);
            }

            if (path == $"/{_authOptions.BypassConfigPath}/reset")
            {
                return ResetSubject(context);
            }

            return UseFakeSubjectToByPassAuthz(context);
        }

        private async Task ResetSubject(HttpContext context)
        {
            _currentSub = null;

            context.Response.StatusCode = 200;
            context.Response.ContentType = "text/string";

            await context.Response.WriteAsync("Subject set to none. Token required for protected endpoints.");
        }

        private async Task SetSubjectFromQueryString(HttpContext context)
        {
            var sub = context.Request.Query["sub"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(sub))
            {
                _currentSub = sub;
            }

            context.Response.StatusCode = 200;
            context.Response.ContentType = "text/string";

            await context.Response.WriteAsync($"Subject set to {_currentSub}");
        }

        private async Task UseFakeSubjectToByPassAuthz(HttpContext context)
        {
            var currentSub = _currentSub;

            var authHeader = context.Request.Headers["Authorization"];

            if (authHeader != StringValues.Empty)
            {
                var header = authHeader.FirstOrDefault();

                if (header != null && !string.IsNullOrWhiteSpace(header) && header.StartsWith("Email ") &&
                    header.Length > "Email ".Length)
                {
                    currentSub = header.Substring("Email ".Length);
                }
            }

            if (!string.IsNullOrWhiteSpace(currentSub))
            {
                context.User = context.User.BuildTestIdentityAllScopeAccess(_authOptions, currentSub);
            }

            await _next.Invoke(context);
        }
    }
}
