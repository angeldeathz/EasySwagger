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
            foreach (var description in _apiVersionDescription.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = Options.ProjectName,
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