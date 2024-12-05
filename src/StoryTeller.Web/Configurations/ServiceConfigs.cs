using StoryTeller.Core.Interfaces;

namespace StoryTeller.Web.Configurations;

public static class ServiceConfigs
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddFastEndpoints();
        services.SwaggerDocument();
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders("*");
            });
        });

        return services;
    }
}
