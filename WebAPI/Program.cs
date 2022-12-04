
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

namespace WebAPI
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();



            RegisterGameTaskTypes(builder);

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

                // 1. example

                //q.ScheduleJob<HelloJob>(trigger => trigger
                //    .WithIdentity("Combined Configuration Trigger")
                //    .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(1)))
                //    .WithDailyTimeIntervalSchedule(x => x.WithInterval(2, IntervalUnit.Second))
                //    .WithDescription("my awesome trigger configured for a job with single call")
                //);




                //// 2. recurr cycles

                // q.ScheduleJob<CycleJob>(trigger => trigger
                //.WithIdentity("Combined Configuration Trigger")
                //.StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(2)))
                //.WithDailyTimeIntervalSchedule(x => x.WithInterval(1, IntervalUnit.Second))
                //.WithDescription("my awesome trigger configured for a job with single call")
                //);


               // 3.once
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

            // Repos
            builder.Services.AddScoped<IGameRepository, GameRepository>();
            builder.Services.AddScoped<IMainMenuRepository, MainMenuRepository>();
            builder.Services.AddScoped<IGameActionsRepository, GameActionsRepository>();
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<IGameStateRepository, GameStateRepository>();
            builder.Services.AddScoped<IStationRepository, StationRepository>();
            builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
            builder.Services.AddScoped<IChatRepository, ChatRepository>();


            
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

            //Cant use builder after building.
            var app = builder.Build();

            app.UseHttpLogging();

            //Migration
            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var playerContextService = scope.ServiceProvider.GetService<PlayerContext>();
                    //context.Database.EnsureDeleted(); // deocher our recommecner
                    playerContextService.Database.Migrate();
                }

                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred");
                }
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

        private static void RegisterGameTaskTypes(WebApplicationBuilder builder)
        {
            var types = typeof(IGameTask).Assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract
                    && typeof(IGameTask).IsAssignableFrom(type)
                    && CustomAttributeExtensions.GetCustomAttribute<GameTaskAttribute>(type) != null);

            foreach (var type in types)
            {
                builder.Services.AddTransient(type);
            }
        }
    }
}