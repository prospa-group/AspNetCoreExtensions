using System;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Prospa.Extensions.AspNetCore.Hosting.ConfigureOptions;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class StartupValidation
    {
        public static IMvcCoreBuilder AddDefaultValidation(this IMvcCoreBuilder builder, Type type)
        {
            builder.AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining(type));

            builder.Services.AddSingleton<IConfigureOptions<ApiBehaviorOptions>, ProblemJsonApiBehaviourOptionsSetup>();

            return builder;
        }

        public static IMvcCoreBuilder AddDefaultValidation(this IMvcCoreBuilder builder, Action<FluentValidationMvcConfiguration> setupOptions)
        {
            builder.AddFluentValidation(setupOptions);

            builder.Services.AddSingleton<IConfigureOptions<ApiBehaviorOptions>, ProblemJsonApiBehaviourOptionsSetup>();

            return builder;
        }
    }
}