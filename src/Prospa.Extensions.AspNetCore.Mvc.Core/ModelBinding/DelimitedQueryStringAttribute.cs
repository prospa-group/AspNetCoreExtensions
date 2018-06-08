using System;
using Microsoft.AspNetCore.Mvc.Filters;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Mvc.ModelBinding
    // ReSharper restore CheckNamespace
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class DelimitedQueryStringAttribute : Attribute, IResourceFilter
    {
        private readonly char[] _delimiters;

        public DelimitedQueryStringAttribute(params char[] delimiters) { _delimiters = delimiters; }

        /// <inheritdoc />
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
        }

        /// <inheritdoc />
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.ValueProviderFactories.AddDelimitedValueProviderFactory(_delimiters);
        }
    }
}
