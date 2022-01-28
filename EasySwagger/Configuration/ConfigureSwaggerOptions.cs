using EasySwagger.CustomSwaggerFilters;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using EasySwagger.Models;

namespace EasySwagger.Configuration
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiVersionDescription;
        public static EasySwaggerOptions Options = new();

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider apiVersionDescription)
        {
            _apiVersionDescription = apiVersionDescription;
        }

        public void Configure(SwaggerGenOptions options)
        {
            options.ResolveConflictingActions(descriptions => descriptions.First());
            options.OperationFilter<RemoveVersionFromParameter>();
            options.DocumentFilter<ReplaceVersionWithExactValueInPath>();

            AddSwaggerDoc(options);
        }

        private void AddSwaggerDoc(SwaggerGenOptions options)
        {
            foreach (var description in _apiVersionDescription.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, GenerateOpenApiInfo(description));
            }
        }

        private OpenApiInfo GenerateOpenApiInfo(ApiVersionDescription description)
        {
            var openApiInfo = new OpenApiInfo
            {
                Description = Options.OpenApiInfo.Description,
                Contact = Options.OpenApiInfo.Contact,
                License = Options.OpenApiInfo.License,
                TermsOfService = Options.OpenApiInfo.TermsOfService,
                Title = Options.OpenApiInfo.Title,
                Version = description.ApiVersion.ToString()
            };

            if (description.IsDeprecated)
            {
                openApiInfo.Description += " - This API version is deprecated";
            }

            return openApiInfo;
        }
    }
}