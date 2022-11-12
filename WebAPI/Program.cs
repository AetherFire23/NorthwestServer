using System;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.HttpLogging;
using WebAPI.Services.ChatService;
using WebAPI.GameState_Management.Game_State_Repository;
using WebAPI.GameTasks;
using System.Reflection;
using WebAPI.Room_template;
using WebAPI.Repository;
using WebAPI.Game_Actions;

namespace WebAPI
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Dependecies
            builder.Services.AddScoped<IPlayerService, PlayerService>();
            builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<IGameStateRepository, GameStateRepository>();
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<IStationRepository, StationRepository>();
            builder.Services.AddScoped<IGameActionsRepository, GameActionsRepository>();
            builder.Services.AddScoped<IMainMenuRepository, MainMenuRepository>();

            RegisterGameTaskTypes(builder);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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