using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using StoryTeller.Infrastructure.Services;
using StoryTeller.Core.Interfaces;
using StoryTeller.Core.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace StoryTeller.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AzureOpenAISettings>(
            configuration.GetSection("AzureOpenAI").Bind);

        services.AddSingleton<IResponseService>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<ResponseService>>();
            return ResponseService.CreateAsync(logger).GetAwaiter().GetResult();
        });

        services.AddSingleton<IAIService>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<AzureOpenAISettings>>();
            var logger = sp.GetRequiredService<ILogger<AzureOpenAIService>>();
            return AzureOpenAIService.CreateAsync(settings, logger).GetAwaiter().GetResult();
        });
        
        // Replace direct registration with factory
        services.AddSingleton<IChatRepository>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<ChatFileRepository>>();
            // Since we can't use async in the factory, we need to wait synchronously
            return ChatFileRepository.CreateAsync(logger).GetAwaiter().GetResult();
        });

        return services;
    }
}
