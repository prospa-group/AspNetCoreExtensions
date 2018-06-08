namespace Prospa.Extensions.AspNetCore.Http
{
    public class HttpErrorLogOptions
    {
        public HttpErrorLogOptions()
        {
            SensitiveValuesFilter = new[] { "Authorization" };
        }

        public string ScrubValue { get; set; }

        public string[] SensitiveValuesFilter { get; set; }
    }
}
