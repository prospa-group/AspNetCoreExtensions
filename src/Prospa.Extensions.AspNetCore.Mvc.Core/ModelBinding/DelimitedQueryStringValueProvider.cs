using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Mvc.ModelBinding
    // ReSharper restore CheckNamespace
{
    public class DelimitedQueryStringValueProvider : QueryStringValueProvider
    {
        private readonly CultureInfo _culture;
        private readonly IQueryCollection _queryCollection;

        public DelimitedQueryStringValueProvider(
            BindingSource bindingSource,
            IQueryCollection values,
            CultureInfo culture,
            char[] delimiters)
            : base(bindingSource, values, culture)
        {
            _queryCollection = values;
            _culture = culture;
            Delimiters = delimiters;
        }

        public char[] Delimiters { get; }

        public override ValueProviderResult GetValue(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var values = _queryCollection[key];

            if (values.Count == 0)
            {
                return ValueProviderResult.None;
            }

            if (values.Any(x => Delimiters.Any(d => !string.IsNullOrWhiteSpace(x) && x.Contains(d))))
            {
                var stringValues = new StringValues(values.SelectMany(x => x.Split(Delimiters, StringSplitOptions.RemoveEmptyEntries)).ToArray());
                return new ValueProviderResult(stringValues, _culture);
            }

            return new ValueProviderResult(values, _culture);
        }
    }
}
