using Microsoft.Extensions.Hosting;

namespace Sandbox.Api
{
    public static class ProgramHealth
    {
        public static IHostBuilder UseDefaultHealth(this IHostBuilder webHostBuilder)
        {
            return webHostBuilder;
        }

        public static IHostBuilder ConfigureDefaultHealth(this IHostBuilder webHostBuilder)
        {
            // TODO: MS Health checks
            return webHostBuilder;
        }
    }
}
