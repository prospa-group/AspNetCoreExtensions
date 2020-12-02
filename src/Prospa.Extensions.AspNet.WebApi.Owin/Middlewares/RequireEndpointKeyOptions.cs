namespace Prospa.Extensions.AspNet.WebApi.Owin.Middlewares
{
    public class RequireEndpointKeyOptions
    {
        public string[] Endpoints { get; set; } = null;

        public string KeyName { get; set; } = null;

        public string Key { get; set; }
    }
}