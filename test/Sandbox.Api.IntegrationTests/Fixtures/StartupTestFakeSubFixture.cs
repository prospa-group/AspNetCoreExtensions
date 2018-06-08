using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Sandbox.Api.IntegrationTests.Fixtures
{
    public class StartupTestFakeSubFixture : IDisposable
    {
        private readonly TestServer _server;

        public HttpClient Client { get; }

        public StartupTestFakeSubFixture()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            Client = _server.CreateClient();

            Client.GetAsync("noauth?sub=123").GetAwaiter().GetResult();

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, "Bearer 123");
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Client?.Dispose();
            _server?.Dispose();
        }
    }
}
