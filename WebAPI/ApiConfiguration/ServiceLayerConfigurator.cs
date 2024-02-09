using WebAPI.Authentication;
using WebAPI.Repositories;
using WebAPI.Services;
namespace WebAPI.ApiConfiguration;

public static class ServiceLayerConfigurator
{
    public static void AddServicesLayer(WebApplicationBuilder builder)
    {
        // Dependencies
        _ = builder.Services.AddScoped<CycleManagerService>();

        // Services
        _ = builder.Services.AddScoped<PlayerService>();
        _ = builder.Services.AddScoped<GameTaskService>();
        _ = builder.Services.AddScoped<PlayerService>();
        _ = builder.Services.AddScoped<GameStateService>();
        _ = builder.Services.AddScoped<ChatService>();
        _ = builder.Services.AddScoped<GameMakerService>();
        _ = builder.Services.AddScoped<ShipService>();

        //landmasses
        _ = builder.Services.AddScoped<LandmassService>();
        _ = builder.Services.AddScoped<LandmassCardsService>();
        _ = builder.Services.AddScoped<UserService>();

        //builder.Services.AddSingleton<ISSEClientManager, GameSSEClientManager>();

        //seeding
        _ = builder.Services.AddScoped<AuthenticationService>();
        _ = builder.Services.AddScoped<LobbyService>();
        _ = builder.Services.AddScoped<LobbyRepository>();

        // sse 
    }
}
