using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Prospa.Extensions.AspNetCore.Mvc.Versioning.Swagger.Routing;

namespace Sandbox.Api.Controllers.V2
{
    [VersionedRoute("[controller]"), V2]
    [ApiController]
    public class DelimitedQueryStringController : ControllerBase
    {
        [HttpGet]
        [DelimitedQueryString(',')]
        public ActionResult<IEnumerable<int>> Get([FromQuery]IEnumerable<int> ids) { return Ok(ids); }
    }
}