using System;
using System.Linq;
using EasySwagger.DocumentFilters;
using EasySwagger.OperationFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EasySwagger.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services,
            Action<EasySwaggerOptions> options = null)
        {
            // set easy swagger options
            SetOptions(options);

            services.AddApiVersioning();
            services.AddVersionedApiExplorer(opt => opt.GroupNameFormat = "'V'V");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(x =>
            {
                x.ResolveConflictingActions(descriptions => descriptions.First());
                x.OperationFilter<RemoveVersionFromParameter>();
                x.DocumentFilter<ReplaceVersionWithExactValueInPath>();
            });
            return services;
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app,
            IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger(opt => opt.RouteTemplate = "{documentName}/swagger.json");

            app.UseSwaggerUI(opt =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    opt.SwaggerEndpoint($"/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
            });

            return app;
        }

        private static void SetOptions(Action<EasySwaggerOptions> options)
        {
            var defaultOptions = new EasySwaggerOptions();
            options?.Invoke(defaultOptions);
            ConfigureSwaggerOptions.Options = defaultOptions;
        }
    }
}