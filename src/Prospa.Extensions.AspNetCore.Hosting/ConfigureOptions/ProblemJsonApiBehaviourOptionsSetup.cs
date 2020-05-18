using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Prospa.Extensions.AspNetCore.Hosting.ConfigureOptions
{
    public class ProblemJsonApiBehaviourOptionsSetup : IConfigureOptions<ApiBehaviorOptions>
    {
        /// <inheritdoc />
        public void Configure(ApiBehaviorOptions options)
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                                     {
                                         Instance = context.HttpContext.Request.Path,
                                         Status = StatusCodes.Status400BadRequest,
                                         Detail = "Please refer to the errors property for additional details."
                                     };

                return new BadRequestObjectResult(problemDetails)
                       {
                           ContentTypes = { "application/problem+json" }
                       };
            };
        }
    }
}
