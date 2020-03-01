using System;
using System.Net;
using Microsoft.Extensions.Logging;

namespace Prospa.Extensions.AspNetCore.Http.DelegatingHandlers
{
    public class LoggingDelegatingHandlerOptions
    {
        public bool DisableErrorRequestBodyLogging { get; set; }

        public bool DisableErrorResponseBodyLogging { get; set; }

        public LogLevel UnsuccessfulResponseLogLevel { get; set; } = LogLevel.Error;

        /// <summary>
        /// Status codes in this array won't be logged as unsuccessful requests.
        /// </summary>
        public HttpStatusCode[] IgnoreStatusCodes { get; set; } = Array.Empty<HttpStatusCode>();
    }
}