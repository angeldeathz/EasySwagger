using System.Linq;
using EasySwagger.CustomSwaggerFilters;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

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

            foreach (var description in _apiVersionDescription.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, GenerateOpenApiInfo(description));
            }
        }

        private OpenApiInfo GenerateOpenApiInfo(ApiVersionDescription description)
        {
            //var info = Options.OpenApiInfo;
            //info.Version = description.ApiVersion.ToString();

            var info = new OpenApiInfo
            {
                Title = "asdasd",
                Version = description.ApiVersion.ToString()
            };

            if (description.IsDeprecated)
            {
                info.Description += "This API version is deprecated";
            }

            return info;
        }
    }
}