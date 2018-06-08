using System;
using Microsoft.AspNetCore.Http;
using Prospa.Extensions.AspNetCore.Http;
using Serilog;
using Serilog.Events;

namespace Prospa.Extensions.AspNetCore.Serilog
{
    public class HttpRequestDetailsSerilogLogger : IHttpRequestDetailsLogger
    {
        private static readonly Func<HttpContext, string> MessageTemplate = httpContext => $"{httpContext.Request.Method} {httpContext.Request.Path.ToString()} failed, Status code: {httpContext.Response.StatusCode}";
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpRequestDetailsSerilogLogger(HttpErrorLogOptions errorLogOptions, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

            var requestLogContext = new RequestDetail(_httpContextAccessor.HttpContext, errorLogOptions);
            _logger = Log.Logger.ForContext(nameof(RequestDetail), requestLogContext, true);
        }

        /// <inheritdoc />
        public void Error(Exception exception)
        {
            _logger.Write(LogEventLevel.Error, exception, MessageTemplate(_httpContextAccessor.HttpContext));
        }

        /// <inheritdoc />
        public void Fatal(Exception exception)
        {
            _logger.Write(LogEventLevel.Fatal, exception, MessageTemplate(_httpContextAccessor.HttpContext));
        }

        /// <inheritdoc />
        public void Warning(Exception exception)
        {
            _logger.Write(LogEventLevel.Warning, exception, MessageTemplate(_httpContextAccessor.HttpContext));
        }
    }
}
