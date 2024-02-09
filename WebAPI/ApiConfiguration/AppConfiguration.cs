using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using WebAPI.Exceptions;
namespace WebAPI.ApiConfiguration;

public static class AppConfiguration
{
    public static void AddAuth(WebApplication app)
    {
        _ = app.UseAuthentication();
        _ = app.UseAuthorization();
        ConfigureExceptions(app);
    }

    public static async Task SeedAndMigrate(WebApplication app)
    {
        //Migration
        using (IServiceScope scope = app.Services.CreateScope())
        {
            PlayerContext? playerContextService = scope.ServiceProvider.GetService<PlayerContext>();
            //  var authContext = scope.ServiceProvider.GetService<AuthenticationContext>();
            try
            {
                // this deletes and recreates the whole databse when it is launched.
                _ = playerContextService.Database.EnsureDeleted(); // deocher our recommecner
                playerContextService.Database.Migrate();

                //   authContext.Database.EnsureDeleted(); // deocher our recommecner
                //   authContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                ILogger<Program> logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred");
            }
        }
    }


    private static void ConfigureExceptions(WebApplication app)
    {
        _ = app.UseExceptionHandler(exceptionHandler =>
        {
            exceptionHandler.Run(async context =>
            {
                IExceptionHandlerPathFeature? exceptionHandlerFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                if (exceptionHandlerFeature?.Error is RequestException exBase)
                {
                    context.Response.StatusCode = (int)exBase.StatusCode;

                    ExceptionResponseData responseData = new()
                    {
                        Data = exBase.ExceptionData,
                        Message = exBase.HttpMessage,
                    };

                    await context.Response.WriteAsJsonAsync(responseData);
                }
            });
        });
    }
}
