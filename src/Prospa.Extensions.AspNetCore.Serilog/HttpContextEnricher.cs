using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Prospa.Extensions.AspNetCore.Serilog
{
    public class HttpContextEnricher : ILogEventEnricher
    {
        private readonly LogEventProperty _correlationIdProperty;
        private readonly LogEventProperty _originalForProperty;
        private readonly LogEventProperty _subProperty;
        private readonly LogEventProperty _clientIdProperty;

        public HttpContextEnricher(HttpContext context)
        {
            _correlationIdProperty = context.Request.Headers.CorrelationIdLogEventProperty();
            _originalForProperty = context.Request.Headers.OriginalForLogEventProperty();
            _subProperty = context.User?.SubjectIdEventProperty();
            _clientIdProperty = context.User?.SubjectIdEventProperty();
        }

        /// <inheritdoc />
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsentAndNotNull(_correlationIdProperty);
            logEvent.AddPropertyIfAbsentAndNotNull(_originalForProperty);
            logEvent.AddPropertyIfAbsentAndNotNull(_subProperty);
            logEvent.AddPropertyIfAbsentAndNotNull(_correlationIdProperty);
        }
    }
}