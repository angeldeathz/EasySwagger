using Microsoft.OpenApi.Models;

namespace EasySwagger.Configuration
{
    public class EasySwaggerOptions
    {
        public EasySwaggerOptions()
        {
            OpenApiInfo = new OpenApiInfo
            {
                Title = "My Project",
                Description = "This is my project description"
            };
        }

        public OpenApiInfo OpenApiInfo { get; set; }
        public string XmlCommentsPath { get; set; }
    }
}