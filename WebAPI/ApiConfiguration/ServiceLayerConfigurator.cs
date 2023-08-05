
using WebAPI.Authentication;
using WebAPI.Interfaces;
using WebAPI.Repository;
using WebAPI.Seeding;
using WebAPI.Services;
using WebAPI.SSE;
using WebAPI.SSE.Senders;

namespace WebAPI.ApiConfiguration;

public static class ServiceLayerConfigurator
{
    public static void AddServicesLayer(WebApplicationBuilder builder)
    {
        // Dependencies
        _ = builder.Services.AddScoped<ICycleManagerService, CycleManagerService>();

        // Services
        _ = builder.Services.AddScoped<IPlayerService, PlayerService>();
        _ = builder.Services.AddScoped<IGameTaskService, GameTaskService>();
        _ = builder.Services.AddScoped<IPlayerService, PlayerService>();
        _ = builder.Services.AddScoped<IFriendService, FriendService>();
        _ = builder.Services.AddScoped<IGameStateService, GameStateService>();
        _ = builder.Services.AddScoped<IChatService, ChatService>();
        _ = builder.Services.AddScoped<IGameMakerService, GameMakerService>();
        _ = builder.Services.AddScoped<IShipService, ShipService>();

        //landmasses
        _ = builder.Services.AddScoped<ILandmassService, LandmassService>();
        _ = builder.Services.AddScoped<ILandmassCardsService, LandmassCardsService>();
        _ = builder.Services.AddScoped<IUserService, UserService>();

        //builder.Services.AddSingleton<ISSEClientManager, GameSSEClientManager>();
        _ = builder.Services.AddScoped<IGameSSESender, GameSSESender>();

        //seeding
        _ = builder.Services.AddScoped<ISeedSequence, SeedSequence>();
        _ = builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

        _ = builder.Services.AddScoped<ILobbyService, LobbyService>();
        _ = builder.Services.AddScoped<ILobbyRepository, LobbyRepository>();

        // sse 
        _ = builder.Services.AddScoped<IGameSSESender, GameSSESender>();
        _ = builder.Services.AddScoped<IMainMenuSSESender, MainMenuSSESender>();
    }
}
