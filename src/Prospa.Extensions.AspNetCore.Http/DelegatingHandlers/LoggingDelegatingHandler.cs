using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Prospa.Extensions.AspNetCore.Http.DelegatingHandlers
{
    public class LoggingDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger<LoggingDelegatingHandler> _logger;
        private readonly LoggingDelegatingHandlerOptions _options;

        public LoggingDelegatingHandler(ILogger<LoggingDelegatingHandler> logger, LoggingDelegatingHandlerOptions options)
        {
            _logger = logger;
            _options = options;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var logLevel = _options.StatusCodeLogLevelOverrides?.ContainsKey(response.StatusCode) == true
                    ? _options.StatusCodeLogLevelOverrides[response.StatusCode]
                    : _options.UnsuccessfulResponseLogLevel;

                var requestString = _options.DisableErrorRequestBodyLogging || request.Content == null
                    ? string.Empty
                    : await request.Content.ReadAsStringAsync().ConfigureAwait(false);
                var responseString = _options.DisableErrorResponseBodyLogging || response.Content == null
                    ? string.Empty
                    : await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                _logger.Log(
                    logLevel,
                    "http request unsuccessful {Method} requestURI: {RequesetURI} requestContent: {RequestContent} response: {StatusCode} {Response}",
                    request.Method.ToString(),
                    request.RequestUri,
                    requestString,
                    response.StatusCode,
                    responseString);
            }

            return response;
        }
    }
}
