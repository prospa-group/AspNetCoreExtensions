using Microsoft.AspNetCore.Mvc;
using Prospa.Extensions.AspNetCore.Mvc.Core.Filters;
using Sandbox.Api.Routing;

namespace Sandbox.Api.Controllers.V2
{
    [VersionedRoute("[controller]"), V2]
    [ApiController]
    public class HttpHeaderController : ControllerBase
    {
        private const string HttpHeaderDescription = "Test Http Header";

        [HttpGet("optional")]
        [HttpHeader("X-Test", HttpHeaderDescription)]
        public ActionResult GetOptional()
        {
            return Ok();
        }

        [HttpGet("require")]
        [HttpHeader("X-Test", HttpHeaderDescription, Required = true)]
        public ActionResult GetRequire()
        {
            return Ok();
        }

        [HttpGet("forward")]
        [HttpHeader("X-Test", HttpHeaderDescription, Forward = true)]
        public ActionResult GetForward()
        {
            return Ok();
        }
    }
}