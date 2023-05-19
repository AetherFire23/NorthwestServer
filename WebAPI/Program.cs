
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpLogging;
using WebAPI.GameTasks;
using System.Reflection;
using WebAPI.Repository;
using WebAPI.Game_Actions;
using WebAPI.Interfaces;
using Quartz;
using WebAPI.Jobs;
using WebAPI.Services;
using Shared_Resources.GameTasks;
using System.Linq;
using WebAPI.Dummies;
using Shared_Resources.Constants;
using Shared_Resources.Models;
using WebAPI.Scratches;
using WebAPI.Strategies;
using Shared_Resources.Enums;

namespace WebAPI
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            // Initialize the rooms template
            RoomTemplate2.InitializeDefaultReflectedRooms(); // could abstract those 2 if I wanted to waste my fucking time
            StationsTemplate.InitializeDefaultReflectedStations();

            // reflection is incoming for registering roleStrategies









            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            RegisterGameTaskTypes(builder);
            StrategyMapper.RegisterRoleStrategies(builder);

            // Now split API and gameTask stuff // think its not useful
            var unityTasks = typeof(IGameTask).Assembly.GetTypes()
                .Where(type =>
                type.IsClass && !type.IsAbstract
                && typeof(IGameTask).IsAssignableFrom(type)).ToList();

            var webAPITaks = typeof(GameTaskAttribute).Assembly.GetTypes()
                .Where(type =>
                type.IsClass && !type.IsAbstract
                && typeof(IGameTask).IsAssignableFrom(type)
                && CustomAttributeExtensions.GetCustomAttribute<GameTaskAttribute>(type) != null).ToList();




            builder.Services.AddQuartz(q =>
            {
                q.SchedulerId = "Scheduler-Core";

                q.UseMicrosoftDependencyInjectionJobFactory();

                q.UseSimpleTypeLoader();
                q.UseInMemoryStore();
                q.UseDefaultThreadPool(tp =>
                {
                    tp.MaxConcurrency = 10;
                });


                q.ScheduleJob<CycleJob>(trigger => trigger
                    .WithIdentity("Combined Configuration Trigger")
                    .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(3)))
                    .WithDescription("my awesome trigger configured for a job with single call"));

            });

            builder.Services.AddQuartzServer(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Dependencies
            builder.Services.AddScoped<ICycleManagerService, CycleManagerService>();

            // Services
            builder.Services.AddScoped<IPlayerService, PlayerService>();
            builder.Services.AddScoped<IGameTaskService, GameTaskService>();
            builder.Services.AddScoped<IPlayerService, PlayerService>();
            builder.Services.AddScoped<IFriendService, FriendService>();
            builder.Services.AddScoped<IGameStateService, GameStateService>();
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<IGameMakerService, GameMakerService>();
            builder.Services.AddScoped<IShipStasusesService, ShipStatusesService>();



            // Repos
            builder.Services.AddScoped<IGameRepository, GameRepository>();
            builder.Services.AddScoped<IMainMenuRepository, MainMenuRepository>();
            builder.Services.AddScoped<IGameActionsRepository, GameActionsRepository>();
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<IGameStateRepository, GameStateRepository>();
            builder.Services.AddScoped<IStationRepository, StationRepository>();
            builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
            builder.Services.AddScoped<IChatRepository, ChatRepository>();
            builder.Services.AddScoped<IGameMakerRepository, GameMakerRepository>();
            builder.Services.AddScoped<ILandmassRepository, LandmassRepository>();
            builder.Services.AddScoped<IShipStatusesRepository, ShipStatusesRepository>();

            //landmasses
            builder.Services.AddScoped<ILandmassService2, LandmassService2>();
            builder.Services.AddScoped<ILandmassCardsRepository, LandmassCardsRepository>();
            builder.Services.AddScoped<ILandmassCardsService, LandmassCardsService>();



            // Configure Automapper
            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            // Injection jobs
            builder.Services.AddTransient<CycleJob>();

            //Add db context here
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<PlayerContext>(options
           => options.UseSqlServer(connectionString));

            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All;
                logging.RequestHeaders.Add("sec-ch-ua");
                logging.ResponseHeaders.Add("MyResponseHeader");
                logging.MediaTypeOptions.AddText("application/javascript");
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;

            });


            // Cant use builder after building.
            var app = builder.Build();

            app.UseHttpLogging();





            //Migration
            using (var scope = app.Services.CreateScope())
            {
                var playerContextService = scope.ServiceProvider.GetService<PlayerContext>();

                try
                {
                    // this deletes and recreates the whole databse when it is launched.
                    playerContextService.Database.EnsureDeleted(); // deocher our recommecner
                    playerContextService.Database.Migrate();



                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred");
                }



                var gameMakerService = scope.ServiceProvider.GetService<IGameMakerService>();
                ILandmassService2? landmassService2 = scope.ServiceProvider.GetService<ILandmassService2>();
                var landmassCardService = scope.ServiceProvider.GetService<ILandmassCardsService>();

                await gameMakerService.CreateDummyGame();
                gameMakerService.InsertVeryDummyValues();

                // some other very dummy values
                var p = playerContextService.Players.FirstOrDefault(x => x.Id == DummyValues.defaultPlayer1Guid);
                playerContextService.Logs.Add(DummyValues.SomeLog(p.CurrentGameRoomId));

                var t = scope.ServiceProvider.GetService<MedicRoleStrategyService>();

                var sz2 = StrategyMapper.GetStrategyTypeByRole(RoleType.Medic);
                var sz3 = StrategyMapper.GetStrategyTypeByRole(RoleType.Commander);
                // real scratch
                // landmassCardService.InitializeLandmassCards(p.GameId).Wait();
                // landmassService2.AdvanceToNextLandmass(p.GameId).Wait();


                playerContextService.SaveChanges();

            }



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();


        }

        //private static void RegisterGameTaskTypes(WebApplicationBuilder builder)
        //{
        //    var types = typeof(IGameTask).Assembly.GetTypes()
        //        .Where(type => type.IsClass && !type.IsAbstract
        //            && typeof(IGameTask).IsAssignableFrom(type)
        //            && CustomAttributeExtensions.GetCustomAttribute<GameTaskAttribute>(type) != null);

        //    foreach (var type in types)
        //    {
        //        builder.Services.AddTransient(type);
        //    }
        //}

        private static void RegisterGameTaskTypes(WebApplicationBuilder builder)
        {
            var apiTypes = typeof(Program).Assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract
                && typeof(IGameTask).IsAssignableFrom(type)
                && CustomAttributeExtensions.GetCustomAttribute<GameTaskAttribute>(type) != null);

            foreach (var type in apiTypes)
            {
                builder.Services.AddTransient(type);
            }
        }
    }
}