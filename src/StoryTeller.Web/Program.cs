using Serilog;
using StoryTeller.Core.Interfaces;
using StoryTeller.Infrastructure;
using StoryTeller.Web.Configurations;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((context, loggerConfig) =>
            loggerConfig.ReadFrom.Configuration(context.Configuration));

        builder.Services
            .AddWebServices()
            .AddMediatrServices()
            .AddInfrastructureServices(builder.Configuration);

        var app = builder.Build();

        // Initialize services that require async initialization
        await using (var scope = app.Services.CreateAsyncScope())
        {
            // These services are just being retrieved but not awaited
            var chatRepo = scope.ServiceProvider.GetRequiredService<IChatRepository>();
            var aiService = scope.ServiceProvider.GetRequiredService<IAIService>();
            var responseService = scope.ServiceProvider.GetRequiredService<IResponseService>();
        }

        app.Use(async (context, next) =>
        {
            context.Response.Headers.Remove("X-XSS-Protection");
            context.Response.Headers.Remove("Expires");
            await next();
        });

        app.Use(async (context, next) =>
        {
            context.Response.Headers.Append("Content-Security-Policy", 
                "frame-ancestors 'self'");
            
            context.Response.Headers.Append("Cache-Control", 
                "no-cache, no-store, must-revalidate");
            
            await next();
        });

        app.UseAppMiddleware();

        await app.RunAsync();
    }
}
