
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
        builder.Services.AddScoped<IGameSSESender, GameSSESender>();

        //seeding
        builder.Services.AddScoped<ISeedSequence, SeedSequence>();
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

        builder.Services.AddScoped<ILobbyService, LobbyService>();
        builder.Services.AddScoped<ILobbyRepository, LobbyRepository>();

        // sse 
        builder.Services.AddScoped<IGameSSESender, GameSSESender>();
        builder.Services.AddScoped<IMainMenuSSESender, MainMenuSSESender>();
    }
}
