using Microsoft.OpenApi;

namespace SelfGrind.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("bearerAuth", doc)] = []
            });
        });
        builder.Services.AddEndpointsApiExplorer();

    }
}