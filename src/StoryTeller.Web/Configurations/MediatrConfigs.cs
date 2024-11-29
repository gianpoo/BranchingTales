using Ardalis.SharedKernel;
using StoryTeller.Core.PromptAggregate;
using StoryTeller.UseCases.Prompts.Create;
using MediatR;
using System.Reflection;

namespace StoryTeller.Web.Configurations;

public static class MediatrConfigs
{
  public static IServiceCollection AddMediatrConfigs(this IServiceCollection services)
  {
    var mediatRAssemblies = new[]
      {
        Assembly.GetAssembly(typeof(Prompt)), // Core
        Assembly.GetAssembly(typeof(CreatePromptCommand)) // UseCases
      };

    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

    return services;
  }
}
