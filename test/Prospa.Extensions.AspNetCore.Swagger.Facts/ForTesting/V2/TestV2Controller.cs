using Microsoft.AspNetCore.Mvc;

namespace Prospa.Extensions.AspNetCore.Swagger.Facts.ForTesting.V2
{
    [Route("v{version:apiVersion}/")]
    [ApiVersion("v2")]
    public class TestV2Controller : ControllerBase
    {
        public IActionResult Get() { return Ok(); }
    }
}
