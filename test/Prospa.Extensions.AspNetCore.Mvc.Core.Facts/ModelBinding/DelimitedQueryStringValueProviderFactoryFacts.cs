using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace Prospa.Extensions.AspNetCore.Mvc.Core.Facts.ModelBinding
{
    public class DelimitedQueryStringValueProviderFactoryFacts
    {
        [Fact]
        public async Task Get_value_provider_returns_deliminated_query_string_provider_instance_with_invariant_culture()
        {
            // Arrange
            var request = new Mock<HttpRequest>();
            request.SetupGet(f => f.Query).Returns(Mock.Of<IQueryCollection>());
            var context = new Mock<HttpContext>();
            context.SetupGet(c => c.Items).Returns(new Dictionary<object, object>());
            context.SetupGet(c => c.Request).Returns(request.Object);
            var actionContext = new ActionContext(context.Object, new RouteData(), new ActionDescriptor());
            var factoryContext = new ValueProviderFactoryContext(actionContext);
            var factory = new DelimitedQueryStringValueProviderFactory();

            // Act
            await factory.CreateValueProviderAsync(factoryContext);

            // Assert
            var valueProvider = Assert.IsType<DelimitedQueryStringValueProvider>(Assert.Single(factoryContext.ValueProviders));
            Assert.Equal(CultureInfo.InvariantCulture, valueProvider.Culture);
        }
    }
}
