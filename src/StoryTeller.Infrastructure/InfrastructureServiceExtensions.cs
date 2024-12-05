using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using StoryTeller.Infrastructure.Services;
using StoryTeller.Core.Interfaces;

namespace StoryTeller.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IResponseService, ResponseService>();
        services.AddSingleton<IChatRepository, ChatFileRepository>();
        return services;
    }
}
