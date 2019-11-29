using System.Security.Claims;
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

        public static LogEventProperty SubjectIdEventProperty(this ClaimsPrincipal user)
        {
            var subjectId = user?.FindFirst("sub")?.Value;
            return subjectId != null
                ? new LogEventProperty(Constants.LogEventProperties.Sub, new ScalarValue(subjectId))
                : null;
        }

        public static LogEventProperty ClientIdEventProperty(this ClaimsPrincipal user)
        {
            var clientId = user?.FindFirst("client_id")?.Value;
            return clientId != null
                ? new LogEventProperty(Constants.LogEventProperties.ClientId, new ScalarValue(clientId))
                : null;
        }
    }
}