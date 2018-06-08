using System;

namespace Prospa.Extensions.AspNetCore.Http
{
    public interface IHttpRequestDetailsLogger
    {
        void Error(Exception exception);

        void Fatal(Exception exception);

        void Warning(Exception exception);
    }
}
