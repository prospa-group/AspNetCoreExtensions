using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Prospa.Extensions.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

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

                    // TODO: Open API
                    // options.SwaggerVersionedDoc(apiVersionDescriptionProvider, assemblyDescription, assembly.GetName().Name);
                    // options.AllowFilteringDocsByApiVersion();

                    AddDefaultOptions(options, assembly);
                    AddDefaultOperationFilters(provider, options);
                    AddDefaultSchemaFilters(options);
                    AddDefaultDocumentFilters(options);

                    options.OperationFilter<AnnotationsOperationFilter>();
                    options.OperationFilter<XmlCommentsOperationFilter>();
                    // options.OperationFilter<AddHeaderOperationFilter>();
                    // options.OperationFilter<SecurityRequirementsOperationFilter>();
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
                        // TODO: Open API
                        // swagger.Host = httpReq.Host.Value;
                        // swagger.LowercaseRoutes();
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
                    // TODO: Open API
                    // options.SwaggerVersionedJsonEndpoints(provider);
                });

            return app;
        }

        private static void AddDefaultDocumentFilters(SwaggerGenOptions options)
        {
            // TODO: Open API
            // options.DocumentFilter<SetVersionInPaths>();
        }

        private static void AddDefaultOperationFilters(IServiceProvider provider, SwaggerGenOptions options)
        {
            var authzOptions = provider.GetRequiredService<AuthOptions>();

            // TODO: Open API
            // options.OperationFilter<AddAuthorizationHeaderParameterOperationFilter>(authzOptions.ScopePolicies);
            // options.OperationFilter<RemoveVersionParameters>();
            // options.OperationFilter<HttpHeaderOperationFilter>();
            // options.OperationFilter<ForbiddenResponseOperationFilter>();
            // options.OperationFilter<UnauthorizedResponseOperationFilter>();
            // options.OperationFilter<DelimitedQueryStringOperationFilter>();
            // options.OperationFilter<DeprecatedVersionOperationFilter>();
        }

        private static void AddDefaultOptions(SwaggerGenOptions options, Assembly assembly)
        {
            options.IgnoreObsoleteActions();
            options.IgnoreObsoleteProperties();
            options.DescribeAllEnumsAsStrings();
            options.DescribeAllParametersInCamelCase();
            options.DescribeStringEnumsInCamelCase();
            // TODO: Open API
            // options.IncludeXmlCommentsIfExists(assembly);
        }

        private static void AddDefaultSchemaFilters(SwaggerGenOptions options)
        {
            // TODO: Open API
            // options.SchemaFilter<ModelStateDictionarySchemaFilter>();
        }
    }
}