using Northwest.Domain.Models;
using Northwest.WebApi.ApiConfiguration;
using Northwest.WebApi.ApiConfiguration.BuilderConfig;
namespace Northwest.WebApi;

public class Program
{
    static async Task Main(string[] args) // techniquement je devrais tout move dans les petites classes
    {
        RoomsTemplate.InitializeDefaultReflectedRooms(); // could abstract those 2 if I wanted to waste my fucking time
        StationsTemplate.InitializeDefaultReflectedStations();
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        ApplicationBuilderHelper.ConfigureWebApplication(builder);
        WebApplication app = builder.Build();
        app.UseHttpLogging();

        await app.Services.SeedAndMigrate2();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}