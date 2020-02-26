using Microsoft.Extensions.Logging;

namespace Prospa.Extensions.AspNetCore.Http.DelegatingHandlers
{
    public class LoggingDelegatingHandlerOptions
    {
        public bool DisableErrorRequestBodyLogging { get; set; }

        public bool DisableErrorResponseBodyLogging { get; set; }

        public LogLevel UnsuccessfulResponseLogLevel { get; set; } = LogLevel.Error;
    }
}