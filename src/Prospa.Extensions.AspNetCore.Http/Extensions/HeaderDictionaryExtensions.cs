#if NETCOREAPP
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Primitives;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Http
// ReSharper restore CheckNamespace
{
    public static class HeaderDictionaryExtensions
    {
        private static readonly string SrubValue = "***REDACTED***";

        public static Dictionary<string, StringValues> ToScrubbedDictionary(
            this IHeaderDictionary headers,
            string scrubValue,
            params string[] nameFilter)
        {
            var result = new Dictionary<string, StringValues>();
            var scrub = string.IsNullOrWhiteSpace(scrubValue) ? SrubValue : scrubValue;

            if (headers == null || headers.Count == 0)
            {
                return result;
            }

            foreach (var name in headers.Keys)
            {
                var value = headers[name];

                if (value.Count > 0)
                {
                    if (nameFilter.Contains(name))
                    {
                        value = scrub;
                    }

                    result.Add(name, value);
                }
            }

            return result;
        }
    }
}
#endif