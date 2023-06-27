
using Shared_Resources.Models;
using WebAPI.ApiConfiguration;
using Shared_Resources.Constants.Endpoints;
using WebAPI.Conventions;

namespace WebAPI
{
    public class Program
    {
        static async Task Main(string[] args) // techniquement je devrais tout move dans les petites classes
        {
            var endp = EndpointPathsMapper.GetFullEndpoint(typeof(MainMenuEndpoints), MainMenuEndpoints.State);


            RoomsTemplate.InitializeDefaultReflectedRooms(); // could abstract those 2 if I wanted to waste my fucking time
            StationsTemplate.InitializeDefaultReflectedStations();






            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(options =>
            {
                options.Conventions.Add(new LowercaseControllerModelConvention());
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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