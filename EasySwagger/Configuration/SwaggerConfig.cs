﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using EasySwagger.CustomSwaggerFilters;

namespace EasySwagger.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services,
            Action<EasySwaggerOptions> customOptions = null)
        {
            // get customOptions swagger
            var optionsSwagger = GetOptions(customOptions);
            ConfigureSwaggerOptions.Options = optionsSwagger;

            services.AddApiVersioning();
            services.AddVersionedApiExplorer(opt => opt.GroupNameFormat = "'V'V");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(x =>
            {
                x.ResolveConflictingActions(descriptions => descriptions.First());
                x.OperationFilter<RemoveVersionFromParameter>();
                x.DocumentFilter<ReplaceVersionWithExactValueInPath>();
                IncludeXmlComments(x, optionsSwagger);
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

        private static EasySwaggerOptions GetOptions(Action<EasySwaggerOptions> options)
        {
            var defaultOptions = new EasySwaggerOptions();
            options?.Invoke(defaultOptions);
            return defaultOptions;
        }

        private static void IncludeXmlComments(SwaggerGenOptions swaggerGenOptions, EasySwaggerOptions options)
        {
            if (string.IsNullOrEmpty(options.XmlCommentsPath) ||
                string.IsNullOrWhiteSpace(options.XmlCommentsPath)) return;

            swaggerGenOptions.IncludeXmlComments(options.XmlCommentsPath);
        }
    }
}