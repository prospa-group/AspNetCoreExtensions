using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Newtonsoft.Json;
using Prospa.Extensions.AspNetCore.Mvc.Versioning.Swagger.OperationFilters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace Prospa.Extensions.AspNetCore.Swagger.Facts.OperationFilters
{
    public class RemoveVersionParametersFacts
    {
        private readonly Operation _operation;
        private readonly OperationFilterContext _context;
        private readonly RemoveVersionParameters _sut;

        public RemoveVersionParametersFacts()
        {
            _operation = new Operation { Parameters = new List<IParameter> { new BodyParameter { Name = "version" } } };
            _context = new OperationFilterContext(new ApiDescription(), new SchemaRegistry(new JsonSerializerSettings()));
            _sut = new RemoveVersionParameters();
        }

        [Fact]
        public void Should_remove_version_parameters_from_operation_body_parameter()
        {
            // Arrange

            // Act
            _sut.Apply(_operation, _context);

            // Assert
            _operation.Parameters.Should().NotContain("version");
        }

        [Fact]
        public void Should_remove_version_parameters_from_operation_non_body_paraamter()
        {
            // Arrange

            // Act
            _sut.Apply(_operation, _context);

            // Assert
            _operation.Parameters.Should().NotContain("version");
        }
    }
}
