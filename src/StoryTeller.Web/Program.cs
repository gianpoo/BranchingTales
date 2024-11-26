using StoryTeller.Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

var logger = Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

logger.Information("Starting web host");

builder.AddLoggerConfigs();

var appLogger = new SerilogLoggerFactory(logger)
    .CreateLogger<Program>();

builder.Services.AddOptionConfigs(builder.Configuration, appLogger, builder);
builder.Services.AddServiceConfigs(appLogger, builder);

builder.Services.AddCors(options =>
{
  options.AddDefaultPolicy(policy =>
  {
    // Allow your frontend origin to make requests
    policy.WithOrigins("http://localhost:50338")  // Replace with your frontend URL
          .AllowAnyMethod()                      // Allow any HTTP method (GET, POST, PUT, DELETE)
          .AllowAnyHeader()                      // Allow any header, including Content-Type
          .AllowCredentials();                   // Allow cookies (if needed)
  });
});

builder.Services.AddFastEndpoints()
    .SwaggerDocument(o =>
    {
      o.ShortSchemaNames = true;
    });

var app = builder.Build();

// Use custom middleware (if any)
await app.UseAppMiddleware();

// Use CORS
app.UseCors();

app.Run();

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program { }
