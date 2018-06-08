using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Prospa.Extensions.AspNetCore.Swagger.Facts.ForTesting.V1
{
    [Route("v{version:apiVersion}/")]
    [ApiVersion("v2")]
    [Authorize]
    public class TestV1Controller : ControllerBase
    {
        public IActionResult Get() { return Ok(); }
    }
}
