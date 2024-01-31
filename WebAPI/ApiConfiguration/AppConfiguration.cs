using Microsoft.EntityFrameworkCore;
namespace WebAPI.ApiConfiguration;

public static class AppConfiguration
{
    public static void AddAuth(WebApplication app)
    {
        _ = app.UseAuthentication();
        _ = app.UseAuthorization();
    }

    public static async Task SeedAndMigrate(WebApplication app)
    {
        //Migration
        using (var scope = app.Services.CreateScope())
        {
            var playerContextService = scope.ServiceProvider.GetService<PlayerContext>();
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
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred");
            }
        }
    }
}
