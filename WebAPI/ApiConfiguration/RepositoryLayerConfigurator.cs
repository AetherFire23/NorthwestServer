using WebAPI.Authentication;
using WebAPI.Game_Actions;
using WebAPI.Interfaces;
using WebAPI.Repository;
using WebAPI.Repository.Users;

namespace WebAPI.ApiConfiguration;

public class RepositoryLayerConfigurator
{
    public static void AddRepositoriesLayer(WebApplicationBuilder builder)
    {
        // Repos
        _ = builder.Services.AddScoped<IGameRepository, GameRepository>();
        _ = builder.Services.AddScoped<IMainMenuRepository, MainMenuRepository>();
        _ = builder.Services.AddScoped<IGameActionsRepository, GameActionsRepository>();
        _ = builder.Services.AddScoped<IRoomRepository, RoomRepository>();
        _ = builder.Services.AddScoped<IGameStateRepository, GameStateRepository>();
        _ = builder.Services.AddScoped<IStationRepository, StationRepository>();
        _ = builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
        _ = builder.Services.AddScoped<IChatRepository, ChatRepository>();
        _ = builder.Services.AddScoped<IGameMakerRepository, GameMakerRepository>();
        _ = builder.Services.AddScoped<ILandmassRepository, LandmassRepository>();
        _ = builder.Services.AddScoped<IShipRepository, ShipRepository>();

        //landmasses
        _ = builder.Services.AddScoped<ILandmassCardsRepository, LandmassCardsRepository>();
        _ = builder.Services.AddScoped<IUserRepository, UserRepository>();

        _ = builder.Services.AddScoped<IJwtTokenManager, JwtTokenManager>();
    }
}
