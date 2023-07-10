using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.AspNetCore;
using Shared_Resources.Entities;
using Shared_Resources.GameTasks;
using System.Reflection;
using System.Text;
using WebAPI.Authentication;
using WebAPI.Constants;
using WebAPI.Conventions;
using WebAPI.Game_Actions;
using WebAPI.GameTasks;
using WebAPI.Interfaces;
using WebAPI.Jobs;
using WebAPI.Repository;
using WebAPI.Repository.Users;
using WebAPI.Services;
using WebAPI.SSE;
using WebAPI.Strategies;

namespace WebAPI.ApiConfiguration
{
    public static class ServicesConfiguration
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            ConfigureConventions(builder);
            ConfigureSwagger(builder);

            RegisterGameTaskTypes(builder);
            RoleStrategyMapper.RegisterRoleStrategies(builder);
            SkillStrategyMapper.RegisterSkillStrategies(builder);
            SSERegistar.RegisterSSEManagerTypes(builder.Services);


            ConfigureQuartz(builder);

            AddServicesLayer(builder);
            AddRepositoriesLayer(builder);

            ConfigureAutoMapper(builder);
            ConfigureDbContext(builder);
            ConfigureHTTPLogging(builder);

            // ConfigureIdentityContext(builder);
            ConfigureJWT(builder);
        }

        private static void RegisterGameTaskTypes(WebApplicationBuilder builder) // should extract this to GameTaskStrategyMapper
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

        private static void ConfigureConventions(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers(options =>
            {
                options.Conventions.Add(new LowercaseControllerModelConvention());
            });
        }

        private static void ConfigureSwagger(WebApplicationBuilder builder)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            // Add Swagger services
            builder.Services.AddSwaggerGen(c =>
            {
                // Add the security definition for JWT Bearer token
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                // Add the security requirement for the JWT Bearer token
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }



        private static void ConfigureQuartz(WebApplicationBuilder builder)
        {
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

                //q.ScheduleJob<CycleJob>(trigger => trigger
                //    .WithIdentity("Combined Configuration Trigger")
                //    .StartNow()
                //        .WithSimpleSchedule(x => x
                //        .WithIntervalInSeconds(5)  // Run every 5 seconds
                //        .RepeatForever())          // Repeat indefinitely
                //    .WithDescription("my awesome trigger configured for a job with single call"));
            });

            builder.Services.AddQuartzServer(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });

            // Add Jobs here
            builder.Services.AddTransient<CycleJob>();
        }

        private static void AddServicesLayer(WebApplicationBuilder builder)
        {
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
            builder.Services.AddScoped<IShipService, ShipService>();

            //landmasses
            builder.Services.AddScoped<ILandmassService, LandmassService>();
            builder.Services.AddScoped<ILandmassCardsService, LandmassCardsService>();
            builder.Services.AddScoped<IUserService, UserService>();

            //builder.Services.AddSingleton<ISSEClientManager, GameSSEClientManager>();
            builder.Services.AddScoped<ISSEManager, SSEManager>();
        }

        private static void AddRepositoriesLayer(WebApplicationBuilder builder)
        {
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
            builder.Services.AddScoped<IShipRepository, ShipRepository>();

            //landmasses
            builder.Services.AddScoped<ILandmassCardsRepository, LandmassCardsRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<IJwtTokenManager, JwtTokenManager>();
        }

        private static void ConfigureAutoMapper(WebApplicationBuilder builder)
        {
            // Configure Automapper
            builder.Services.AddAutoMapper(typeof(Program).Assembly);
        }

        private static void ConfigureDbContext(WebApplicationBuilder builder)
        {
            //Add db context here
            string playerContextConnectionString = builder.Configuration.GetConnectionString("PlayerConnection");
            string authenticationConnectionString = builder.Configuration.GetConnectionString("AuthenticationConnection");
            builder.Services.AddDbContext<PlayerContext>(options
           => options.UseSqlServer(playerContextConnectionString));

            //builder.Services.AddDbContext<AuthenticationContext>(options =>
            //    options.UseSqlServer(authenticationConnectionString));
        }

        private static void ConfigureHTTPLogging(WebApplicationBuilder builder)
        {
            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All;
                logging.RequestHeaders.Add("sec-ch-ua");
                logging.ResponseHeaders.Add("MyResponseHeader");
                logging.MediaTypeOptions.AddText("application/javascript");
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
            });
        }

        private static void ConfigureIdentityContext(WebApplicationBuilder builder)
        {
            builder.Services.AddIdentityCore<User>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 10;
                o.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AuthenticationContext>()
            .AddDefaultTokenProviders();
        }

        private static void ConfigureJWT(WebApplicationBuilder builder)
        {
            var jwtSection = builder.Configuration.GetSection("Jwt");
            var jwtConfig = new JwtConfig();
            jwtSection.Bind(jwtConfig);
            builder.Services.Configure<JwtConfig>(jwtSection);

            // Configure JWT authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Set JWT bearer options
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey))
                };
            });
            builder.Services.AddAuthorization();
        }
    }
}
// will have to configure CORS some day