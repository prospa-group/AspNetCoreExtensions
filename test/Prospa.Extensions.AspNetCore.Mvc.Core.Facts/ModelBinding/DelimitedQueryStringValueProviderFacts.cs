using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;

namespace Prospa.Extensions.AspNetCore.Mvc.Core.Facts.ModelBinding
{
    public class DelimitedQueryStringValueProviderFacts : EnumerableValueProviderTest
    {
        protected override IEnumerableValueProvider GetEnumerableValueProvider(
            BindingSource bindingSource,
            Dictionary<string, StringValues> values,
            CultureInfo culture)
        {
            var backingStore = new QueryCollection(values);
            return new DelimitedQueryStringValueProvider(bindingSource, backingStore, culture, new[] { ',' });
        }
    }
}
