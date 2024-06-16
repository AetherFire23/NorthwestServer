using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Northwest.Persistence;
namespace Northwest.WebApi.ApiConfiguration;

public static class DbContextConfiguration
{
    public static async Task SeedAndMigrate2(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var playerContextService = scope.ServiceProvider.GetService<PlayerContext>();

        try
        {
            playerContextService.Database.EnsureDeleted(); // deocher our recommecner
            playerContextService.Database.Migrate();
        }
        catch (Exception ex)
        {
            //var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            //logger.LogError(ex, "An error occurred");

            Console.WriteLine("error occurred during migration");
            Console.WriteLine(ex.Message);
        }
    }
}
