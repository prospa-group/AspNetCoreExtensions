using static System.Array;

namespace Prospa.Extensions.AspNetCore.Http.Middlewares
{
    public class DiagnosticActivityMiddlewareOptions
    {
        public bool DisableAddingCorrelationIdAsActivityBaggage { get; set; }

        // <summary>
        // By default the client id from incoming request is added as a tag.
        // Setting this property to true disables this behaviour.
        // </summary>
        public bool DisableClientIdActvityTagging { get; set; }

        public string[] HeadersToTag { get; set; } = Empty<string>();

        public string[] QueryStringValuesToTag { get; set; } = Empty<string>();

        /// <summary>
        ///     array of route values that should be added as diagnostic tags.
        /// </summary>
        public string[] RouteValuesToTag { get; set; } = Empty<string>();
    }
}