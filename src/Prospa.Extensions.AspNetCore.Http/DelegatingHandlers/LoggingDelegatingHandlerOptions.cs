using System;
using System.Collections.Generic;
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
        /// Values in this dictionary will override the log level defined in UnsuccessfulResponseLogLevel.
        /// </summary>
        public Dictionary<HttpStatusCode, LogLevel> StatusCodeLogLevelOverrides { get; set; } = new Dictionary<HttpStatusCode, LogLevel>();
    }
}