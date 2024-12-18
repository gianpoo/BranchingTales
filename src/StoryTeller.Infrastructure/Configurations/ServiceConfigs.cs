using StoryTeller.Core.Settings;

namespace StoryTeller.Infrastructure.Configurations;

public static class ServiceConfigs
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = new AzureOpenAISettings();
        configuration.GetSection("AzureOpenAI").Bind(settings);
        services.AddSingleton(settings);
        return services;
    }
}
