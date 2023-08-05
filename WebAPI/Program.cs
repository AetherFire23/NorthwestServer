
using Newtonsoft.Json;
using Shared_Resources.Constants.Endpoints;
using Shared_Resources.Constants.Mapper;
using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using Shared_Resources.Models.SSE;
using WebAPI.ApiConfiguration;
using WebAPI.Dummies;

namespace WebAPI;

public class Program
{
    static async Task Main(string[] args) // techniquement je devrais tout move dans les petites classes
    {
        string fullPath = GameEndpoints.GetFullPath(GameEndpoints.GameState);

        List<Player> players = new List<Player>()
        {
            DummyValues.Fred,
            DummyValues.Ben,
        };
        var sseData = new SSEData(SSEType.Heartbeat, players);
        string toLine = sseData.ConvertToReadableLine();
        var parsed = SSEClientData.ParseData(toLine);

        var players2 = JsonConvert.DeserializeObject<List<Player>>(parsed.SerializedData);


        var endp = EndpointPathsMapper.GetFullEndpoint(typeof(MainMenuEndpoints), MainMenuEndpoints.State);


        RoomsTemplate.InitializeDefaultReflectedRooms(); // could abstract those 2 if I wanted to waste my fucking time
        StationsTemplate.InitializeDefaultReflectedStations();






        var builder = WebApplication.CreateBuilder(args);





        ApplicationBuilderHelper.ConfigureServices(builder);


        WebApplication app = builder.Build();

        _ = app.UseHttpLogging();



        await AppConfiguration.SeedAndMigrate(app);


        // Configure the HTTP request pipeline.
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