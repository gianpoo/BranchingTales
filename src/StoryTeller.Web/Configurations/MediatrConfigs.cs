using Ardalis.SharedKernel;
using StoryTeller.Core.ChatAggregate;
using StoryTeller.UseCases.Prompts.Create;
using StoryTeller.UseCases.Chats.Create;
using MediatR;
using System.Reflection;

namespace StoryTeller.Web.Configurations;

public static class MediatrConfigs
{
  public static IServiceCollection AddMediatrServices(this IServiceCollection services)
  {
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
      Assembly.GetExecutingAssembly(),
      typeof(CreateChatCommand).Assembly  // UseCases assembly
    ));
    return services;
  }
}
