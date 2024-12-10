using Serilog;
using StoryTeller.Infrastructure;
using StoryTeller.Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddWebServices()
    .AddMediatrServices()
    .AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Response.Headers.Remove("X-XSS-Protection");
    context.Response.Headers.Remove("X-Frame-Options");
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

app.Run();
