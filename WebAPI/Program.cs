
using Shared_Resources.Models;
using WebAPI.ApiConfiguration;
using Shared_Resources.Constants.Endpoints;
using WebAPI.Conventions;
using Shared_Resources.Models.SSE;
using Shared_Resources.Enums;
using Shared_Resources.Entities;
using WebAPI.Dummies;
using Newtonsoft.Json;
using WebAPI.Extensions;

namespace WebAPI
{
    public class Program
    {
        static async Task Main(string[] args) // techniquement je devrais tout move dans les petites classes
        {


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

            



            ServicesConfiguration.ConfigureServices(builder); 


            WebApplication app = builder.Build();

            app.UseHttpLogging();



            await AppConfiguration.SeedAndMigrate(app);


            // Configure the HTTP request pipeline.
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
}