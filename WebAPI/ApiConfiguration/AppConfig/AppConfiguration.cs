using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Northwest.Persistence;
using Northwest.WebApi.Exceptions;
namespace Northwest.WebApi.ApiConfiguration.AppConfig;

public static class AppConfiguration
{
    public static void AddAuth(WebApplication app)
    {
        _ = app.UseAuthentication();
        _ = app.UseAuthorization();
        app.ConfigureHttpRequestExceptions();

    }

    //public static async Task SeedAndMigrate(WebApplication app)
    //{
    //    using var scope = app.Services.CreateScope();
    //    var playerContextService = scope.ServiceProvider.GetService<PlayerContext>();
    //    try
    //    {
    //        // this deletes and recreates the whole databse when it is launched.
    //        playerContextService.Database.EnsureDeleted(); // deocher our recommecner
    //        playerContextService.Database.Migrate();

    //        //   authContext.Database.EnsureDeleted(); // deocher our recommecner
    //        //   authContext.Database.Migrate();
    //    }
    //    catch (Exception ex)
    //    {
    //        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    //        logger.LogError(ex, "An error occurred");
    //    }
    //}
}
