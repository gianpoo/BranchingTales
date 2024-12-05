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

app.UseAppMiddleware();

app.Run();
