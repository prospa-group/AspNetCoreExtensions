using Prospa.Extensions.AspNetCore.Serilog;
using Serilog.Events;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Http
    // ReSharper restore CheckNamespace
{
    public static class LogEventPropertyHeaderDictionaryExtensions
    {
        public static LogEventProperty CorrelationIdLogEventProperty(this IHeaderDictionary headers)
        {
            return headers.ContainsKey(Constants.HeaderKeys.CorrelationId)
                ? new LogEventProperty(Constants.LogEventProperties.CorrelationId, new ScalarValue(headers[Constants.HeaderKeys.CorrelationId]))
                : null;
        }

        public static LogEventProperty OriginalForLogEventProperty(this IHeaderDictionary headers)
        {
            return headers.ContainsKey(Constants.HeaderKeys.OriginalFor)
                ? new LogEventProperty(Constants.LogEventProperties.OriginalFor, new ScalarValue(headers[Constants.HeaderKeys.OriginalFor]))
                : null;
        }
    }
}