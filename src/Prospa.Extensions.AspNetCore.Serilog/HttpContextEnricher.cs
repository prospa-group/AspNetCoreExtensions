using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Prospa.Extensions.AspNetCore.Serilog
{
    public class HttpContextEnricher : ILogEventEnricher
    {
        private readonly LogEventProperty _correlationIdProperty;
        private readonly LogEventProperty _originalForProperty;

        public HttpContextEnricher(HttpContext context)
        {
            _correlationIdProperty = context.Request.Headers.CorrelationIdLogEventProperty();
            _originalForProperty = context.Request.Headers.OriginalForLogEventProperty();
        }

        /// <inheritdoc />
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsentAndNotNull(_correlationIdProperty);
            logEvent.AddPropertyIfAbsentAndNotNull(_originalForProperty);
        }
    }
}