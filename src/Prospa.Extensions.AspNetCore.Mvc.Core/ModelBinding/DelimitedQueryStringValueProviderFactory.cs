using System;
using System.Globalization;
using System.Threading.Tasks;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Mvc.ModelBinding
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     A <see cref="IValueProviderFactory" /> that creates <see cref="IValueProvider" /> instances that
    ///     read optionally delimited values from the request query-string.
    /// </summary>
    public class DelimitedQueryStringValueProviderFactory : IValueProviderFactory
    {
        private static readonly char[] DefaultDelimiters = { ',' };
        private readonly char[] _delimiters;

        public DelimitedQueryStringValueProviderFactory()
            : this(DefaultDelimiters)
        {
        }

        public DelimitedQueryStringValueProviderFactory(params char[] delimiters)
        {
            if (delimiters == null || delimiters.Length == 0)
            {
                _delimiters = DefaultDelimiters;
            }
            else
            {
                _delimiters = delimiters;
            }
        }

        /// <inheritdoc />
        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var valueProvider = new DelimitedQueryStringValueProvider(
                BindingSource.Query,
                context.ActionContext.HttpContext.Request.Query,
                CultureInfo.InvariantCulture,
                _delimiters);

            context.ValueProviders.Add(valueProvider);

            return Task.CompletedTask;
        }
    }
}
