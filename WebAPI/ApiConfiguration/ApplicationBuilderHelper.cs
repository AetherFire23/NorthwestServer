﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Quartz;
using Quartz.AspNetCore;
using System.Text;
using WebAPI.Authentication;
using WebAPI.Conventions;
using WebAPI.GameTasks;
using WebAPI.GameTasks.Reflect;
using WebAPI.Jobs;
using WebAPI.Strategies;
using WebAPI.UniversalSkills;

namespace WebAPI.ApiConfiguration;

public static class ApplicationBuilderHelper
{
    public static void ConfigureWebApplication(WebApplicationBuilder builder)
    {
        ConfigureControllers(builder);
        ConfigureSwagger(builder);

        RegisterGameTaskTypes(builder);
        RoleStrategyMapper.RegisterRoleStrategies(builder);
        SkillStrategyMapper.RegisterSkillStrategies(builder);

        ConfigureQuartz(builder);

        ServiceLayerConfigurator.AddServicesLayer(builder);
        RepositoryLayerConfigurator.AddRepositoriesLayer(builder);

        ConfigureAutoMapper(builder);
        ConfigureDbContext(builder);
        ConfigureHTTPLogging(builder);

        // ConfigureIdentityContext(builder);
        ConfigureJWT(builder);

        builder.Services.AddTaskDelegateCache();
    }

    private static void ConfigureNewtonsoft(WebApplicationBuilder builder)
    {

    }


    // starting not to like this not being inside gameTaskTypeSelector
    private static void RegisterGameTaskTypes(WebApplicationBuilder builder) // should extract this to GameTaskStrategyMapper
    {
        //IEnumerable<Type> apiTypes = typeof(Program).Assembly.GetTypes()
        //    .Where(type => type.IsClass && !type.IsAbstract
        //    && typeof(IGameTask).IsAssignableFrom(type)
        //    && CustomAttributeExtensions.GetCustomAttribute<GameTaskAttribute>(type) != null);
        List<Type> gameTaskTypes = GameTaskTypeSelector.GetTaskTypes();
        foreach (Type? type in gameTaskTypes)
        {
            _ = builder.Services.AddScoped(type);
        }
    }

    private static void ConfigureControllers(WebApplicationBuilder builder) // required for my library for endpoints
    {
        builder.Services.AddControllers(options =>
        {
            options.Conventions.Add(new LowercaseControllerModelConvention());
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
        })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;



                // to convert enum to string and vice-versa to that the codegen can happen correctly 
                options.SerializerSettings.Converters.Add(new StringEnumConverter());


            });
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

            _ = q.ScheduleJob<CycleJob>(trigger => trigger
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
        builder.Services.AddTransient<CycleJob>();
    }

    private static void ConfigureAutoMapper(WebApplicationBuilder builder)
    {
        // Configure Automapper
        // _ = builder.Services.AddAutoMapper(typeof(Program).Assembly);
    }

    private static void ConfigureDbContext(WebApplicationBuilder builder)
    {
        // Add db context here
        string? playerContextConnectionString = builder.Configuration.GetConnectionString("PlayerConnectionSql");

        // useNp
        // https://stackoverflow.com/questions/3582552/what-is-the-format-for-the-postgresql-connection-string-url
        // for parameters https://www.npgsql.org/doc/connection-string-parameters.html
        // Host and Server works
        // DBeaver tick "show all conncetions"
        _ = builder.Services.AddDbContext<PlayerContext>(options =>
            options.UseSqlServer(playerContextConnectionString)
            .EnableSensitiveDataLogging(true)); // only should be appleid to development
    }

    private static void ConfigureHTTPLogging(WebApplicationBuilder builder)
    {
        _ = builder.Services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.All;
            _ = logging.RequestHeaders.Add("sec-ch-ua");
            _ = logging.ResponseHeaders.Add("MyResponseHeader");
            logging.MediaTypeOptions.AddText("application/javascript");
            logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
        });
    }

    //private static void ConfigureIdentityContext(WebApplicationBuilder builder)
    //{
    //    _ = builder.Services.AddIdentityCore<User>(o =>
    //    {
    //        o.Password.RequireDigit = true;
    //        o.Password.RequireLowercase = false;
    //        o.Password.RequireUppercase = false;
    //        o.Password.RequireNonAlphanumeric = false;
    //        o.Password.RequiredLength = 10;
    //        o.User.RequireUniqueEmail = true;
    //    })
    //    .AddEntityFrameworkStores<AuthenticationContext>()
    //    .AddDefaultTokenProviders();
    //}

    private static void ConfigureJWT(WebApplicationBuilder builder)
    {
        IConfigurationSection jwtSection = builder.Configuration.GetSection("Jwt");
        JwtConfig jwtConfig = new JwtConfig();
        jwtSection.Bind(jwtConfig);
        _ = builder.Services.Configure<JwtConfig>(jwtSection);

        // Configure JWT authentication
        _ = builder.Services.AddAuthentication(options =>
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
            };
        });
        _ = builder.Services.AddAuthorization();
    }
}
// will have to configure CORS some day