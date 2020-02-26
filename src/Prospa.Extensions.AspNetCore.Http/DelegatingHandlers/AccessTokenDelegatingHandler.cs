using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Caching.Memory;

namespace Prospa.Extensions.AspNetCore.Http.DelegatingHandlers
{
    /// <summary>
    /// Delegating Handler that encapsulates accessing caching the token.
    /// </summary>
    public class AccessTokenDelegatingHandler : DelegatingHandler
    {
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private readonly IMemoryCache _memoryCache;
        private readonly HttpClient _httpClient;
        private readonly AccessTokenDelegatingHandlerOptions _options;
        private readonly string _cacheKey;
        private bool _disposed;

        public TimeSpan Timeout { get; } = TimeSpan.FromSeconds(5.0);

        public AccessTokenDelegatingHandler(
            HttpClient httpClient,
            IMemoryCache memoryCache,
            AccessTokenDelegatingHandlerOptions options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _options = options ?? throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrEmpty(_options.ClientId))
            {
                throw new ArgumentException("ClientId cannot be null", nameof(options.ClientId));
            }

            if (string.IsNullOrEmpty(_options.TokenEndpoint))
            {
                throw new ArgumentException("token endpoint cannot be null", nameof(options.TokenEndpoint));
            }

            _cacheKey = System.Text.Json.JsonSerializer.Serialize(_options);
        }

        //
        // Summary:
        //     /// Sends an HTTP request to the inner handler to send to the server as an asynchronous
        //     operation. ///
        //
        // Parameters:
        //   request:
        //     The HTTP request message to send to the server.
        //
        //   cancellationToken:
        //     A cancellation token to cancel operation.
        //
        // Returns:
        //     /// Returns System.Threading.Tasks.Task`1. The task object representing the asynchronous
        //     operation. ///
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if ((await GetAccessTokenAsync(cancellationToken).ConfigureAwait(false)).IsMissing() && !await RenewTokensAsync(cancellationToken).ConfigureAwait(false))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    RequestMessage = request
                };
            }

            request.SetBearerToken(await GetAccessTokenAsync(cancellationToken).ConfigureAwait(false));
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            if (response.StatusCode != HttpStatusCode.Unauthorized)
            {
                return response;
            }

            if (!await RenewTokensAsync(cancellationToken).ConfigureAwait(false))
            {
                return response;
            }

            response.Dispose();
            request.SetBearerToken(await GetAccessTokenAsync(cancellationToken).ConfigureAwait(false));
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        //
        // Summary:
        //     /// Releases the unmanaged resources used by the System.Net.Http.DelegatingHandler,
        //     and optionally disposes of the managed resources. ///
        //
        // Parameters:
        //   disposing:
        //     true to release both managed and unmanaged resources; false to releases only
        //     unmanaged resources.
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                _lock.Dispose();
            }

            base.Dispose(disposing);
        }

        private async Task<bool> RenewTokensAsync(CancellationToken cancellationToken)
        {
            if (!await _lock.WaitAsync(Timeout, cancellationToken).ConfigureAwait(false))
            {
                return false;
            }

            try
            {
                var tokenRequest = new ClientCredentialsTokenRequest
                {
                    ClientId = _options.ClientId,
                    ClientSecret = _options.ClientSecret,
                    Method = HttpMethod.Post,
                    Address = _options.TokenEndpoint,
                    Scope = _options.Scope,
                    ClientCredentialStyle = ClientCredentialStyle.PostBody,
                };

                var response = await _httpClient.RequestClientCredentialsTokenAsync(tokenRequest, cancellationToken).ConfigureAwait(false);

                if (!response.IsError)
                {
                    // leave some buffer for expiry
                    // ExpiresIn is in seconds
                    var expiry = response.ExpiresIn > 60
                        ? TimeSpan.FromSeconds(response.ExpiresIn - 60)
                        : TimeSpan.FromSeconds(30);

                    _memoryCache.Set(
                        key: _cacheKey,
                        value: response.AccessToken,
                        absoluteExpirationRelativeToNow: expiry);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        private async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            if (!await _lock.WaitAsync(Timeout, cancellationToken).ConfigureAwait(false))
            {
                return string.Empty;
            }

            try
            {
                // returns null if cache has been invalidated
                return _memoryCache.Get<string>(_cacheKey);
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}