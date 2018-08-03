using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Prospa.Extensions.AspNetCore.Authorization;
using Prospa.Extensions.AspNetCore.Mvc.Versioning.Swagger.DocumentFilters;
using Prospa.Extensions.AspNetCore.Mvc.Versioning.Swagger.OperationFilters;
using Prospa.Extensions.AspNetCore.Swagger;
using Prospa.Extensions.AspNetCore.Swagger.OperationFilters;
using Prospa.Extensions.AspNetCore.Swagger.SchemaFilters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Builder
    // ReSharper restore CheckNamespace
{
    public static class StartupSwagger
    {
        public static IServiceCollection AddDefaultSwagger(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            services.AddSwaggerGen(
                options =>
                {
                    var assembly = typeof(StartupSwagger).GetTypeInfo().Assembly;
                    var assemblyDescription = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
                    var apiVersionDescriptionProvider = provider.GetRequiredService<IApiVersionDescriptionProvider>();

                    options.SwaggerVersionedDoc(apiVersionDescriptionProvider, assemblyDescription, assembly.GetName().Name);
                    options.AllowFilteringDocsByApiVersion();

                    AddDefaultOptions(options, assembly);
                    AddDefaultOperationFilters(provider, options);
                    AddDefaultSchemaFilters(options);
                    AddDefaultDocumentFilters(options);
                });

            return services;
        }

        public static IApplicationBuilder UseDefaultSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger(
                options =>
                {
                    options.PreSerializeFilters.Add((swagger, httpReq) =>
                    {
                        swagger.Host = httpReq.Host.Value;
                        swagger.LowercaseRoutes();
                    });
                });

            return app;
        }

        public static IApplicationBuilder UseDefaultSwaggerUi(this IApplicationBuilder app)
        {
            app.UseSwaggerUI(
                options =>
                {
                    var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
                    options.SwaggerVersionedJsonEndpoints(provider);
                });

            return app;
        }

        private static void AddDefaultDocumentFilters(SwaggerGenOptions options) { options.DocumentFilter<SetVersionInPaths>(); }

        private static void AddDefaultOperationFilters(IServiceProvider provider, SwaggerGenOptions options)
        {
            var authzOptions = provider.GetRequiredService<AuthOptions>();

            options.OperationFilter<AddAuthorizationHeaderParameterOperationFilter>(authzOptions.ScopePolicies);
            options.OperationFilter<RemoveVersionParameters>();
            options.OperationFilter<HttpHeaderOperationFilter>();
            options.OperationFilter<ForbiddenResponseOperationFilter>();
            options.OperationFilter<UnauthorizedResponseOperationFilter>();
            options.OperationFilter<DelimitedQueryStringOperationFilter>();
            options.OperationFilter<DeprecatedVersionOperationFilter>();
        }

        private static void AddDefaultOptions(SwaggerGenOptions options, Assembly assembly)
        {
            options.IgnoreObsoleteActions();
            options.IgnoreObsoleteProperties();
            options.DescribeAllEnumsAsStrings();
            options.DescribeAllParametersInCamelCase();
            options.DescribeStringEnumsInCamelCase();
            options.IncludeXmlCommentsIfExists(assembly);
        }

        private static void AddDefaultSchemaFilters(SwaggerGenOptions options)
        {
            options.SchemaFilter<ModelStateDictionarySchemaFilter>();
        }
    }
}