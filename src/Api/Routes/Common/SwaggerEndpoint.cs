using Microsoft.OpenApi.Models;

namespace Onion.Api.Routes.Common;

public class SwaggerEndpoint : IRouteEndpoint, IRouteService
{
    public void DefineEndpoints(WebApplication app)
    {
        if(app.Environment.IsProduction())
        {
            return;
        }

        app.UseSwagger();
        app.UseSwaggerUI();
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Description = "Bearer Authentication with JSON Web Token",
                Type = SecuritySchemeType.Http
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {new OpenApiSecurityScheme
                {Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }}, new List<string>()}
            });
        });
    }
}
