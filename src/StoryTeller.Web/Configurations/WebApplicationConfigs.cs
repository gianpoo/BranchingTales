using Ardalis.ListStartupServices;
using FastEndpoints;
using FastEndpoints.Swagger;

namespace StoryTeller.Web.Configurations;

public static class WebApplicationConfigs
{
  public static IApplicationBuilder UseAppMiddleware(this WebApplication app)
  {
    if (app.Environment.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
    }
    else
    {
      app.UseDefaultExceptionHandler();
      app.UseHsts();
    }

    app.UseCors();
    
    app.UseFastEndpoints()
        .UseSwaggerGen();

    app.UseHttpsRedirection();

    return app;
  }
}
