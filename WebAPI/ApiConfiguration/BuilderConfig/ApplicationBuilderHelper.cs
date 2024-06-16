using Quartz;
using Microsoft.OpenApi.Models;
using Northwest.Domain.Initialization;
using Northwest.Domain.Jobs;
using Northwest.WebApi.Conventions;
using Quartz.AspNetCore;
using Northwest.Domain.Models;

namespace Northwest.WebApi.ApiConfiguration.BuilderConfig;

public static class ApplicationBuilderHelper
{
    public static void ConfigureWebApplication(WebApplicationBuilder builder)
    {
        builder.Services.InitializeDomainServices();

        ConfigureControllers(builder);
        ConfigureSwagger(builder);
        builder.Services.ConfigureHTTPLogging();
        ConfigureQuartz(builder);
        builder.ConfigureJWTHeaders();
        builder.Services.AddAuthorization();
    }

    private static void ConfigureControllers(WebApplicationBuilder builder) // required for my library for endpoints
    {
        builder.Services.AddControllers(options =>
        {
            options.Conventions.Add(new LowercaseControllerModelConvention());
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
        }).ConfigureControllerSerialization();
    }

    private static void ConfigureSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGenNewtonsoftSupport();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        // Add Swagger services

        builder.Services.AddSwaggerGen(c =>
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

            q.ScheduleJob<CycleTickJob>(trigger => trigger
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
        builder.Services.AddTransient<CycleTickJob>();
    }
}