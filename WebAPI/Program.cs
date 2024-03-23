using WebAPI.ApiConfiguration;
using WebAPI.Models;
namespace WebAPI;

public class Program
{
    static async Task Main(string[] args) // techniquement je devrais tout move dans les petites classes
    {

        RoomsTemplate.InitializeDefaultReflectedRooms(); // could abstract those 2 if I wanted to waste my fucking time
        StationsTemplate.InitializeDefaultReflectedStations();
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        ApplicationBuilderHelper.ConfigureWebApplication(builder);

        WebApplication app = builder.Build();
        _ = app.UseHttpLogging();
        await AppConfiguration.SeedAndMigrate(app);

        if (app.Environment.IsDevelopment())
        {
            _ = app.UseSwagger();
            _ = app.UseSwaggerUI();
        }
   
        _ = app.UseHttpsRedirection();

        _ = app.UseAuthentication();
        _ = app.UseAuthorization();

        _ = app.MapControllers();

        app.Run();
    }
}