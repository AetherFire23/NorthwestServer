using Microsoft.OpenApi.Models;
using Northwest.Domain.GameTasks;
using Northwest.Domain.GameTasks.Initialization;
using Northwest.Domain.Jobs;
using Northwest.Domain.Strategies;
using Northwest.Domain.UniversalSkills;
using Northwest.WebApi.Conventions;
using Quartz;
using Quartz.AspNetCore;

namespace Northwest.WebApi.ApiConfiguration.BuilderConfig;

public static class ApplicationBuilderHelper
{
    public static void ConfigureWebApplication(WebApplicationBuilder builder)
    {
        // Still domain
        RoleStrategyMapper.RegisterRoleStrategies(builder.Services);
        SkillStrategyMapper.RegisterSkillStrategies(builder.Services);
        ServiceLayerConfigurator.AddServicesLayer(builder);
        RepositoryLayerConfigurator.AddRepositoriesLayer(builder);
        builder.Services.ConfigureDbContext();
        builder.Services.RegisterGameTaskTypes();


        ConfigureControllers(builder);
        ConfigureSwagger(builder);
        builder.Services.ConfigureHTTPLogging();
        ConfigureQuartz(builder);
        builder.ConfigureJWTHeaders();
        builder.Services.AddAuthorization();
    }

    // starting not to like this not being inside gameTaskTypeSelector
    //private static void RegisterGameTaskTypes(WebApplicationBuilder builder) // should extract this to GameTaskStrategyMapper
    //{
    //    //IEnumerable<Type> apiTypes = typeof(Program).Assembly.GetTypes()
    //    //    .Where(type => type.IsClass && !type.IsAbstract
    //    //    && typeof(IGameTask).IsAssignableFrom(type)
    //    //    && CustomAttributeExtensions.GetCustomAttribute<GameTaskAttribute>(type) != null);
    //    List<Type> gameTaskTypes = GameTaskTypeSelector.GetTaskTypes();
    //    foreach (Type? type in gameTaskTypes)
    //    {
    //        _ = builder.Services.AddScoped(type);
    //    }
    //}

    private static void ConfigureControllers(WebApplicationBuilder builder) // required for my library for endpoints
    {
        builder.Services.AddControllers(options =>
        {
            options.Conventions.Add(new LowercaseControllerModelConvention());
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
        })
            .ConfigureControllerSerialization();
    }

    private static void ConfigureSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGenNewtonsoftSupport();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        _ = builder.Services.AddEndpointsApiExplorer();
        // Add Swagger services

        _ = builder.Services.AddSwaggerGen(c =>
        {
            c.UseAllOfToExtendReferenceSchemas();
            // Add the security definition for JWT Bearer token
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
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
                            Id = "Bearer",
                        },
                    },
                    Array.Empty<string>()
                },
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

            _ = q.ScheduleJob<CycleTickJob>(trigger => trigger
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

        _ = builder.Services.AddQuartzServer(options =>
        {
            // when shutting down we want jobs to complete gracefully
            options.WaitForJobsToComplete = true;
        });

        // Add Jobs here
        builder.Services.AddTransient<CycleTickJob>();
    }

    private static void ConfigureAutoMapper(WebApplicationBuilder builder)
    {
        // Configure Automapper
        // _ = builder.Services.AddAutoMapper(typeof(Program).Assembly);
    }

    //private static void ConfigureDbContext(WebApplicationBuilder builder)
    //{
    //    // Add db context here
    //    string? playerContextConnectionString = builder.Configuration.GetConnectionString("PlayerConnectionSql");

    //    // useNp
    //    // https://stackoverflow.com/questions/3582552/what-is-the-format-for-the-postgresql-connection-string-url
    //    // for parameters https://www.npgsql.org/doc/connection-string-parameters.html
    //    // Host and Server works
    //    // DBeaver tick "show all conncetions"
    //    //_ = builder.Services.AddDbContext<PlayerContext>(options =>
    //    //    options.UseSqlServer(playerContextConnectionString)
    //    //    .EnableSensitiveDataLogging(true)); // only should be appleid to development

    //    builder.Services.AddDbContext<PlayerContext>();

    //}
}
// will have to configure CORS some day