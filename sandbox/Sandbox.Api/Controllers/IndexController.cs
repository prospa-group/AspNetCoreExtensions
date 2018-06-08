using Microsoft.AspNetCore.Mvc;

namespace Sandbox.Api.Controllers
{
    [Route("")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiVersionNeutral]
    public class IndexController : ControllerBase
    {
        /// <summary>
        /// Redirects to the swagger page.
        /// </summary>
        /// <returns>A 301 Moved Permanently response.</returns>
        [HttpGet]
        public IActionResult Index() => RedirectPermanent("/swagger");
    }
}
