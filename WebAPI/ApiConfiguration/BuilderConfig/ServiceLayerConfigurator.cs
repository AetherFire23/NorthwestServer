using Northwest.Domain.Authentication;
using Northwest.Domain.GameTasks;
using Northwest.Domain.Repositories;
using Northwest.Domain.Services;

namespace Northwest.WebApi.ApiConfiguration;

public static class ServiceLayerConfigurator
{
    public static void AddServicesLayer(WebApplicationBuilder builder)
    {
        // Dependencies
        builder.Services.AddScoped<CycleManagerService>();

        // Services
        builder.Services.AddScoped<PlayerService>();
        builder.Services.AddScoped<GameTaskService>();
        builder.Services.AddScoped<PlayerService>();
        builder.Services.AddScoped<GameStateService>();
        builder.Services.AddScoped<ChatService>();
        builder.Services.AddScoped<GameMakerService>();
        builder.Services.AddScoped<ShipService>();

        //landmasses
        builder.Services.AddScoped<UserService>();

        //builder.Services.AddSingleton<ISSEClientManager, GameSSEClientManager>();

        //seeding
        _ = builder.Services.AddScoped<AuthenticationService>();
        _ = builder.Services.AddScoped<LobbyService>();
        _ = builder.Services.AddScoped<LobbyRepository>();

        // sse 
    }
}
