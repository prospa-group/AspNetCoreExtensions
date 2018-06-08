using Microsoft.Extensions.Primitives;

namespace Prospa.Extensions.AspNetCore.Mvc.Core.Filters
{
    /// <summary>
    ///     Require the X-Correlation-ID HTTP header to be specified in a request.
    /// </summary>
    /// <seealso cref="HttpHeaderAttribute" />
    public class CorrelationIdHttpHeaderAttribute : HttpHeaderAttribute
    {
        private const string DefaultHeader = "X-Correlation-ID";
        private const string DefaultDescription = "Used to uniquely identify the HTTP request. This identifier is used to correlate the HTTP request between a client and server.";

        public CorrelationIdHttpHeaderAttribute()
            : base(DefaultHeader, DefaultDescription)
        {
            Forward = true;
            Required = true;
        }

        public override bool IsValid(StringValues headerValues) => !StringValues.IsNullOrEmpty(headerValues);
    }
}