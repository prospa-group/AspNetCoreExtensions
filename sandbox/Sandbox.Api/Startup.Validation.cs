using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sandbox.Api;
using Sandbox.Api.ConfigureOptions;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class StartupValidation
    {
        public static IMvcCoreBuilder AddDefaultValidation(this IMvcCoreBuilder builder)
        {
            builder.AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Startup>());

            builder.Services.AddSingleton<IConfigureOptions<ApiBehaviorOptions>, ProblemJsonApiBehaviourOptionsSetup>();

            return builder;
        }
    }
}