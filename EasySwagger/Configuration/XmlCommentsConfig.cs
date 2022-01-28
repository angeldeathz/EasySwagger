using Microsoft.Extensions.DependencyInjection;

namespace EasySwagger.Configuration
{
    public static class XmlCommentsConfig
    {
        public static IServiceCollection AddSwaggerXmlComments(this IServiceCollection services, string xmlPath)
        {
            services.ConfigureSwaggerGen(x =>
            {
                if (string.IsNullOrEmpty(xmlPath) ||
                    string.IsNullOrWhiteSpace(xmlPath)) return;

                x.IncludeXmlComments(xmlPath);
            });

            return services;
        }
    }
}