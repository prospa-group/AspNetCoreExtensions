using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Newtonsoft.Json;
using Prospa.Extensions.AspNetCore.Mvc.Versioning.Swagger.DocumentFilters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace Prospa.Extensions.AspNetCore.Swagger.Facts.DocumentFilters
{
    public class SetVersionInPathsFacts
    {
        [Fact]
        public void Should_place_version_token_with_actual_version()
        {
            // Arrange
            var swaggerDoc = new SwaggerDocument
                             {
                                Paths = new Dictionary<string, PathItem>
                                        {
                                            { "{version}/value", new PathItem() }
                                        },
                                Info = new Info
                                       {
                                           Version = "v1"
                                       }
                             };
            var context = new DocumentFilterContext(
                new ApiDescriptionGroupCollection(new List<ApiDescriptionGroup>(), 1),
                new SchemaRegistry(new JsonSerializerSettings()));

            var sut = new SetVersionInPaths();

            // Act
            sut.Apply(swaggerDoc, context);

            // Assert
            swaggerDoc.Paths.Keys.Single().Should().Be("v1/value");
        }
    }
}
