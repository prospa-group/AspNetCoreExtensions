using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Prospa.Extensions.AspNetCore.Http
{
    public class RequestDetail
    {
        public RequestDetail(HttpContext context, HttpErrorLogOptions options)
        {
            var headers = options.SensitiveValuesFilter == null ||
                          options.SensitiveValuesFilter.Length == 0
                ? context.Request.Headers.ToDictionary(h => h.Key, h => h.Value)
                : context.Request.Headers.ToScrubbedDictionary(
                    options.ScrubValue,
                    options.SensitiveValuesFilter);

            QueryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : string.Empty;
            Method = context.Request.Method;
            Cookies = context.Request.Cookies.Select(c => c.Key);
            Headers = headers;
            Host = context.Request.Host.ToUriComponent();
            Protocol = context.Request.Protocol;

            if (context.Request.HasFormContentType)
            {
                FormKeys = context.Request.Form.Select(v => v.Key);
            }
        }

        public IEnumerable<string> Cookies { get; }

        public IEnumerable<string> FormKeys { get; }

        public IDictionary<string, StringValues> Headers { get; }

        public string Host { get; }

        public string Method { get; }

        public string Protocol { get; }

        public string QueryString { get; }
    }
}
