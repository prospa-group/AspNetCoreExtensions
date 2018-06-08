using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Sandbox.Api.IntegrationTests.Fixtures
{
    public class StartupTestFixture : IDisposable
    {
        private readonly TestServer _server;

        public HttpClient Client { get; }

        public StartupTestFixture()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            Client = _server.CreateClient();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Client?.Dispose();
            _server?.Dispose();
        }
    }
}
