using Microsoft.EntityFrameworkCore;
using Northwest.Persistence;
using System.Runtime.CompilerServices;

namespace Northwest.WebApi.ApiConfiguration;

public static class DbContextConfiguration
{
    public static async Task SeedAndMigrate(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var playerContextService = scope.ServiceProvider.GetService<PlayerContext>();
        try
        {
            playerContextService.Database.EnsureDeleted(); // deocher our recommecner
            playerContextService.Database.Migrate();
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred");
        }
    }

    public static void ConfigureDbContext(this IServiceCollection serviceCollection)
    {
        // Add db context here
        //string? playerContextConnectionString = builder.Configuration.GetConnectionString("PlayerConnectionSql");

        // useNp
        // https://stackoverflow.com/questions/3582552/what-is-the-format-for-the-postgresql-connection-string-url
        // for parameters https://www.npgsql.org/doc/connection-string-parameters.html
        // Host and Server works
        // DBeaver tick "show all conncetions"
        //_ = builder.Services.AddDbContext<PlayerContext>(options =>
        //    options.UseSqlServer(playerContextConnectionString)
        //    .EnableSensitiveDataLogging(true)); // only should be appleid to development

        serviceCollection.AddDbContext<PlayerContext>();

    }
}
