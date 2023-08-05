using Microsoft.EntityFrameworkCore;
using WebAPI.Interfaces;

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

            _ = scope.ServiceProvider.GetService<IGameMakerService>();
            _ = scope.ServiceProvider.GetService<ILandmassService>();
            _ = scope.ServiceProvider.GetService<ILandmassCardsService>();

            //await gameMakerService.CreateDummyGame();
            //gameMakerService.InsertVeryDummyValues();

            // some other very dummy values
            //await landmassService2.AdvanceToNextLandmass(DummyValues.defaultGameGuid);
            //playerContextService.SaveChanges();

            var seedSequence = scope.ServiceProvider.GetService<ISeedSequence>();
            await seedSequence.ExecuteSeedSequence();
        }
    }
}
