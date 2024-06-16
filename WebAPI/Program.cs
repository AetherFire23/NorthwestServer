using DomainTests.AdditionalServices;
using DomainTests.GameStart;
using Northwest.Domain.Models;
using Northwest.WebApi.ApiConfiguration;
using Northwest.WebApi.ApiConfiguration.BuilderConfig;
namespace Northwest.WebApi;

public class Program
{
    static async Task Main(string[] args) // techniquement je devrais tout move dans les petites classes
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.RegisterTestServices();
        ApplicationBuilderHelper.ConfigureWebApplication(builder);



        var app = builder.Build();

        await app.Services.SeedAndMigrate2();

        using (var scope = app.Services.CreateScope())
        {
            var serv = scope.ServiceProvider.GetRequiredService<GameStartService>();

            await serv.CreateUsersAndStartGame();
        }

        app.UseHttpLogging();

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