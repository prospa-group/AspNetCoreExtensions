using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Sandbox.Api.IntegrationTests
{
    public class CorrelationControllerFacts : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public CorrelationControllerFacts(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact(Skip = "WIP")]
        public async Task Should_require_correlation_id()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("v2/correlation/require");

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}